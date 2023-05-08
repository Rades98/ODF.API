using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Navigation
{
	public class NavigationResponseModel : BaseResponseModel
	{
		public NavigationResponseModel(string baseUrl, string countryCode) : base(baseUrl, "/navigation", HttpMethods.Get, countryCode)
		{
		}

		[JsonProperty("login", Required = Required.AllowNull)]
		public NamedAction? LoginAction { get; set; }

		[JsonProperty("menuItems", Required = Required.Always)]
		public ICollection<NamedAction> MenuItems { get; } = new Collection<NamedAction>();

		[JsonProperty("languageMutations", Required = Required.Always)]
		public NamedAction? LanguageMutations { get; set; }

		[JsonProperty("userName", Required = Required.AllowNull)]
		public string UserName { get; set; }

		[JsonProperty("logout", Required = Required.AllowNull)]
		public NamedAction? LogoutAction { get; set; }
	}
}
