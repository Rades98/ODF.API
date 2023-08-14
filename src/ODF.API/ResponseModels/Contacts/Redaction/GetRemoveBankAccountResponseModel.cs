using Newtonsoft.Json;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Contacts.Redaction
{
	public class GetRemoveBankAccountResponseModel
	{
		[JsonProperty("removeBankAccountActions", Required = Required.Always)]
		public IEnumerable<NamedAction> RemoveBankAccountActions { get; set; } = Enumerable.Empty<NamedAction>();

		[JsonProperty("title", Required = Required.Always)]
		public string Title { get; set; } = string.Empty;
	}
}
