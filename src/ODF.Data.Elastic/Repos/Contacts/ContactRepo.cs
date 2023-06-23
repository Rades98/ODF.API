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

			street.AddToScriptWithParamIsfEdited(nameof(Address.Street), scriptParams, script, "ctx._source.address");
			city.AddToScriptWithParamIsfEdited(nameof(Address.City), scriptParams, script, "ctx._source.address");
			postalCode.AddToScriptWithParamIsfEdited(nameof(Address.PostalCode), scriptParams, script, "ctx._source.address");
			country.AddToScriptWithParamIsfEdited(nameof(Address.Country), scriptParams, script, "ctx._source.address");

			return (await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source(script.ToString())
						.Params(scriptParams))
					.Refresh(true), cancellationToken
					)).IsValid;
		}

		public async Task<bool> UpdateContactAsync(string eventName, string eventManager, string email, CancellationToken cancellationToken)
		{
			var scriptParams = new Dictionary<string, object>();
			StringBuilder script = new StringBuilder();

			eventName.AddToScriptWithParamIsfEdited(nameof(Contact.EventName), scriptParams, script);
			eventManager.AddToScriptWithParamIsfEdited(nameof(Contact.EventManager), scriptParams, script);
			email.AddToScriptWithParamIsfEdited(nameof(Contact.Email), scriptParams, script);

			return (await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source(script.ToString())
						.Params(scriptParams))
					.Refresh(true), cancellationToken
					)).IsValid;
		}

		#region bank acc

		public async Task<bool> AddBankAccountAsync(string bank, string accountId, string iban, CancellationToken cancellationToken)
			=> (await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source("if (ctx._source.bankAccounts == null) { ctx._source.bankAccounts = new ArrayList(); } ctx._source.bankAccounts.add(params.elem);")
						.Params(d => d
						.Add("elem", new BankAccount { AccountId = accountId, Bank = bank, IBAN = iban }))
					)
					.Refresh(true), cancellationToken
					)).IsValid;

		public async Task<bool> RemoveBankAccountAsync(string iban, CancellationToken cancellationToken)
			=> (await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source("ctx._source.bankAccounts.removeIf(a -> a.iBAN == params.iban);")
						.Params(d => d
						.Add("iban", iban))
					)
					.Refresh(true), cancellationToken
					)).IsValid;

		#endregion bank acc


		#region contact person

		public async Task<bool> AddContactPersonAsync(ContactPerson person, CancellationToken cancellationToken)
		{
			var last = (await _elasticClient.SearchAsync<Contact>(s => s.Size(1), cancellationToken)).Documents.FirstOrDefault();
			int lastOrder = last is null ? 0 : last.ContactPersons.OrderBy(x => x.Order.Value).LastOrDefault()?.Order ?? 1;

			person.Order = lastOrder + 1;
			person.Id = Guid.NewGuid();

			return (await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source("if (ctx._source.contactPersons == null) { ctx._source.contactPersons = new ArrayList(); } ctx._source.contactPersons.add(params.elem);")
						.Params(d => d
						.Add("elem", person))
					)
					.Refresh(true), cancellationToken
					)).IsValid;
		}

		public async Task<bool> UpdateContactPersonAsync(ContactPerson person, CancellationToken cancellationToken)
		{
			var actualPerson = (await GetAsync(cancellationToken)).ContactPersons.FirstOrDefault(psn => psn.Id == person.Id);

			var scriptParams = new Dictionary<string, object>();
			StringBuilder script = new StringBuilder("for (item in ctx._source.contactPersons) {if (item.id == params.Id) {");
			scriptParams.Add(nameof(ContactPerson.Id), person.Id);

			var allRoles = person.Roles;

			if (allRoles.Any() && actualPerson is not null)
			{
				actualPerson.Roles.ToList().AddRange(person.Roles.ToList());
				allRoles = actualPerson.Roles.Distinct();
			}

			person.Name.AddToScriptWithParamIsfEdited(nameof(person.Name), scriptParams, script, "item");
			person.Title.AddToScriptWithParamIsfEdited(nameof(person.Title), scriptParams, script, "item");
			person.Email.AddToScriptWithParamIsfEdited(nameof(person.Email), scriptParams, script, "item");
			person.Surname.AddToScriptWithParamIsfEdited(nameof(person.Surname), scriptParams, script, "item");
			person.Base64Image.AddToScriptWithParamIsfEdited(nameof(person.Base64Image), scriptParams, script, "item");
			person.Order.AddToScriptWithParamIsfEdited(nameof(person.Order), scriptParams, script, "item");
			allRoles.AddToScriptWithParamIsfEdited(nameof(person.Roles), scriptParams, script, "item");

			script.Append("} }");

			return (await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Size(1)
					.Script(s => s
						.Source(script.ToString())
						.Params(scriptParams))
					.Refresh(true), cancellationToken
					)).IsValid;
		}

		public async Task<bool> RemoveContactPersonAsync(Guid id, CancellationToken cancellationToken)
			=> (await _elasticClient.UpdateByQueryAsync<Contact>(s => s
					.MatchAll()
					.Script(s => s
						.Source("ctx._source.contactPersons.removeIf(a -> a.id == params.Id);")
						.Params(d => d
						.Add("Id", id))
					)
					.Refresh(true), cancellationToken
					)).IsValid;

		#endregion contact person
	}
}
