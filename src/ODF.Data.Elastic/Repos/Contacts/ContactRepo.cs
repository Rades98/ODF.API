using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using ODF.AppLayer.Repos;
using ODF.Domain.Entities.ContactEntities;

namespace ODF.Data.Elastic.Repos.Contacts
{
	public class ContactRepo : IContactRepo
	{
		private readonly IElasticClient _elasticClient;

		public ContactRepo(IElasticClient elasticClient)
		{
			_elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
		}


		public async Task<Contact> GetAsync(CancellationToken cancellationToken)
		{
			var response = await _elasticClient.SearchAsync<Contact>(s => s
							.Size(1), cancellationToken);

			return response.Documents.FirstOrDefault();
		}

		public async Task<IEnumerable<BankAccount>> GetBankAccountsAsync(CancellationToken cancellationToken)
			=> (await GetAsync(cancellationToken)).BankAccounts;

		public async Task<bool> UpdateAddressAsync(string street, string city, string postalCode, string country, CancellationToken cancellationToken)
		{
			var scriptParams = new Dictionary<string, object>();
			StringBuilder script = new StringBuilder();

			if (!string.IsNullOrEmpty(street))
			{
				scriptParams.Add(nameof(Address.Street), street);
				script.Append($"ctx._source.address.street = params.Street;");
			}

			if (!string.IsNullOrEmpty(city))
			{
				scriptParams.Add(nameof(Address.City), city);
				script.Append("ctx._source.address.city = params.City;");
			}

			if (!string.IsNullOrEmpty(postalCode))
			{
				scriptParams.Add(nameof(Address.PostalCode), postalCode);
				script.Append("ctx._source.address.postalCode = params.PostalCode;");
			}

			if (!string.IsNullOrEmpty(country))
			{
				scriptParams.Add(nameof(Address.Country), country);
				script.Append("ctx._source.address.country = params.Country;");
			}

			var response = await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source(script.ToString())
						.Params(scriptParams))
					.Refresh(true), cancellationToken
					);

			return response.IsValid;
		}

		public async Task<bool> UpdateContactAsync(string eventName, string eventManager, string email, CancellationToken cancellationToken)
		{
			var scriptParams = new Dictionary<string, object>();
			StringBuilder script = new StringBuilder();

			if (!string.IsNullOrEmpty(eventName))
			{
				scriptParams.Add(nameof(Contact.EventName), eventName);
				script.Append("ctx._source.eventName = params.EventName;");
			}

			if (!string.IsNullOrEmpty(eventManager))
			{
				scriptParams.Add(nameof(Contact.EventManager), eventManager);
				script.Append("ctx._source.eventManager = params.EventManager;");
			}

			if (!string.IsNullOrEmpty(email))
			{
				scriptParams.Add(nameof(Contact.Email), email);
				script.Append("ctx._source.email = params.Email;");
			}

			var response = await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source(script.ToString())
						.Params(scriptParams))
					.Refresh(true), cancellationToken
					);

			if (response.IsValid)
			{
				var actual = (await _elasticClient.SearchAsync<Contact>(s => s.Size(1), cancellationToken)).Documents.First();

				bool eventNameOk = true, eventManagerOk = true, emailOk = true;

				if (!string.IsNullOrEmpty(eventName))
				{
					eventNameOk = actual.EventName == eventName;
				}

				if (!string.IsNullOrEmpty(eventManager))
				{
					eventManagerOk = actual.EventManager == eventManager;
				}

				if (!string.IsNullOrEmpty(email))
				{
					emailOk = actual.Email == email;
				}

				return eventNameOk && eventManagerOk && emailOk;
			}

			return false;
		}

		#region bank acc

		public async Task<bool> AddBankAccountAsync(string bank, string accountId, string iban, CancellationToken cancellationToken)
		{
			var response = await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source("if (ctx._source.bankAccounts == null) { ctx._source.bankAccounts = new ArrayList(); } ctx._source.bankAccounts.add(params.elem);")
						.Params(d => d
						.Add("elem", new BankAccount { AccountId = accountId, Bank = bank, IBAN = iban }))
					)
					.Refresh(true), cancellationToken
					);

			if (response.IsValid)
			{
				var actual = (await _elasticClient.SearchAsync<Contact>(s => s.Size(1), cancellationToken)).Documents.First();

				return actual.BankAccounts.Any(acc => acc.IBAN == iban);
			}

			return false;
		}

		public async Task<bool> RemoveBankAccountAsync(string iban, CancellationToken cancellationToken)
		{
			var response = await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source("ctx._source.bankAccounts.removeIf(a -> a.iBAN == params.iban);")
						.Params(d => d
						.Add("iban", iban))
					)
					.Refresh(true), cancellationToken
					);

			if (response.IsValid)
			{
				var actual = (await _elasticClient.SearchAsync<Contact>(s => s.Size(1), cancellationToken)).Documents.First();

				return !actual.BankAccounts.Any(acc => acc.IBAN == iban);
			}

			return false;
		}

		#endregion bank acc


		#region contact person

		public async Task<bool> AddContactPersonAsync(ContactPerson person, CancellationToken cancellationToken)
		{
			var last = (await _elasticClient.SearchAsync<Contact>(s => s.Size(1), cancellationToken)).Documents.FirstOrDefault();

			if (last is null)
			{
				return false;
			}

			int lastOrder = last.ContactPersons.OrderBy(x => x.Order.Value).LastOrDefault()?.Order ?? 1;

			person.Order = lastOrder + 1;
			var id = Guid.NewGuid();
			person.Id = id;

			var response = await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source("if (ctx._source.contactPersons == null) { ctx._source.contactPersons = new ArrayList(); } ctx._source.contactPersons.add(params.elem);")
						.Params(d => d
						.Add("elem", person))
					)
					.Refresh(true), cancellationToken
					);

			if (response.IsValid)
			{
				var actual = (await _elasticClient.SearchAsync<Contact>(s => s.Size(1), cancellationToken)).Documents.First();

				return actual.ContactPersons.Any(cp => cp.Id == id);
			}

			return false;
		}

		public async Task<bool> UpdateContactPersonAsync(ContactPerson person, CancellationToken cancellationToken)
		{
			var scriptParams = new Dictionary<string, object>();
			StringBuilder script = new StringBuilder("for (item in ctx._source.contactPersons) {if (item.id == params.Id) {");
			scriptParams.Add(nameof(ContactPerson.Id), person.Id);

			person.Email.AddIfEdited(scriptParams, script);
			person.Title.AddIfEdited(scriptParams, script);
			person.Email.AddIfEdited(scriptParams, script);
			person.Surname.AddIfEdited(scriptParams, script);
			person.Base64Image.AddIfEdited(scriptParams, script);
			person.Order.AddIfEdited(scriptParams, script);
			person.Roles.AddIfEdited(scriptParams, script);

			script.Append("} }");

			var response = await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source(script.ToString())
						.Params(scriptParams))
					.Refresh(true), cancellationToken
					);

			return response.IsValid;
		}

		public async Task<bool> RemoveContactPersonAsync(Guid id, CancellationToken cancellationToken)
		{
			var response = await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source("ctx._source.contactPersons.removeIf(a -> a.id == params.Id);")
						.Params(d => d
						.Add("Id", id))
					)
					.Refresh(true), cancellationToken
					);

			if (response.IsValid)
			{
				var actual = (await _elasticClient.SearchAsync<Contact>(s => s.Size(1), cancellationToken)).Documents.First();

				return !actual.ContactPersons.Any(cp => cp.Id == id);
			}

			return false;
		}

		#endregion contact person


	}
}
