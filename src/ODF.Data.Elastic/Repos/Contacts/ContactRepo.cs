using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using ODF.Data.Contracts.Entities.ContactEntities;
using ODF.Data.Contracts.Interfaces;
using ODF.Data.Elastic;

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
		
		public async Task<bool> UpdateAddressAsync(string street, string city, string postalCode, string country, CancellationToken cancellationToken)
		{
			var scriptParams = new Dictionary<string, object>();
			StringBuilder script = new StringBuilder();

			//TODO Find a better way or FML
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

			if (response.IsValid)
			{
				var actual = (await _elasticClient.SearchAsync<Contact>(s => s.Size(1), cancellationToken)).Documents.First();

				bool streetOk = true, cityOk = true, postalCodeOk = true, countryOk = true;

				if(!string.IsNullOrEmpty(street))
				{
					streetOk = actual.Address.Street == street;
				}

				if (!string.IsNullOrEmpty(city))
				{
					cityOk = actual.Address.City == city;
				}

				if (!string.IsNullOrEmpty(postalCode))
				{
					postalCodeOk = actual.Address.PostalCode == postalCode;
				}

				if (!string.IsNullOrEmpty(country))
				{
					countryOk = actual.Address.Country == country;
				}

				return streetOk && cityOk && postalCodeOk && countryOk;
			}

			return false;
		}

		public async Task<bool> UpdateContactAsync(string eventName, string eventManager, string email, CancellationToken cancellationToken)
		{
			var scriptParams = new Dictionary<string, object>();
			StringBuilder script = new StringBuilder();

			//TODO Find a better way or FML
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

			if(response.IsValid)
			{
				var actual = (await _elasticClient.SearchAsync<Contact>(s => s.Size(1), cancellationToken)).Documents.First();

				bool eventNameOk = true, eventManagerOk = true, emailOk = true;

				if(!string.IsNullOrEmpty(eventName))
				{
					eventNameOk = actual.EventName == eventName;
				}

				if(!string.IsNullOrEmpty(eventManager))
				{
					eventManagerOk = actual.EventManager == eventManager;
				}

				if(!string.IsNullOrEmpty(email))
				{
					emailOk = actual.Email == email;
				}

				return eventNameOk && eventManagerOk && emailOk;
			}

			return false;

			//TODO add some check
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

			var lastOrder = last.ContactPersons.OrderBy(x => x.Order.Value).LastOrDefault()?.Order ?? 1;

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

			//TODO Find a better way or FML
			if (!string.IsNullOrEmpty(person.Email))
			{
				scriptParams.Add(nameof(ContactPerson.Email), person.Email);
				script.Append("item.email = params.Email;");
			}

			if (!string.IsNullOrEmpty(person.Title))
			{
				scriptParams.Add(nameof(ContactPerson.Title), person.Title);
				script.Append("item.title = params.Title;");
			}

			if (!string.IsNullOrEmpty(person.Name))
			{
				scriptParams.Add(nameof(ContactPerson.Name), person.Name);
				script.Append("item.name = params.Name;");
			}

			if (!string.IsNullOrEmpty(person.Surname))
			{
				scriptParams.Add(nameof(ContactPerson.Surname), person.Surname);
				script.Append("item.surname = params.Surname;");
			}

			if (person.Roles.Any())
			{
				scriptParams.Add(nameof(ContactPerson.Roles), person.Roles);
				script.Append("item.roles = params.Roles;");
			}

			if (!string.IsNullOrEmpty(person.Base64Image))
			{
				scriptParams.Add(nameof(ContactPerson.Base64Image), person.Base64Image);
				script.Append("item.base64Image = params.Base64Image;");
			}

			if (person.Order is not null)
			{
				scriptParams.Add(nameof(ContactPerson.Order), person.Order);
				script.Append("item.order = params.Order;");
			}

			script.Append("} }");

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

				Expression<Func<ContactPerson, bool>> exp = c => c.Id == person.Id;

				if (!string.IsNullOrEmpty(person.Email))
				{
					exp = exp.AndAlsoNext(x => x.Email == person.Email);
				}

				if (!string.IsNullOrEmpty(person.Title))
				{
					exp = exp.AndAlsoNext(x => x.Title == person.Title);
				}

				if (!string.IsNullOrEmpty(person.Name))
				{
					exp = exp.AndAlsoNext(x => x.Name == person.Name);
				}

				if (!string.IsNullOrEmpty(person.Surname))
				{
					exp = exp.AndAlsoNext(x => x.Surname == person.Surname);
				}

				if (person.Roles.Any())
				{
					exp = exp.AndAlsoNext(x => x.Roles.SequenceEqual(person.Roles));
				}

				if (!string.IsNullOrEmpty(person.Base64Image))
				{
					exp = exp.AndAlsoNext(x => x.Base64Image == person.Base64Image);
				}

				if (person.Order is not null)
				{
					exp = exp.AndAlsoNext(x => x.Order == person.Order);
				}

				var filter = exp.Compile();
				return actual.ContactPersons.Any(filter);
			}

			return false;
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
