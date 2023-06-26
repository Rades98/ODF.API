using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common;

namespace ODF.API.ResponseModels.Navigation
{
	public class NavigationResponseModel : BaseResponseModel
	{
		public NavigationResponseModel() : base()
		{
		}

		[JsonProperty("login", Required = Required.AllowNull)]
		public NamedAction? LoginAction { get; set; }

		[JsonProperty("menuItems", Required = Required.Always)]
		public ICollection<NamedAction> MenuItems { get; } = new Collection<NamedAction>();

		[JsonProperty("languageMutations", Required = Required.Always)]
		public NamedAction? LanguageMutations { get; set; }

		[JsonProperty("userName", Required = Required.AllowNull)]
		public string? UserName { get; set; } = null;

		[JsonProperty("logout", Required = Required.AllowNull)]
		public NamedAction? LogoutAction { get; set; }

		[JsonProperty("register", Required = Required.AllowNull)]
		public NamedAction? RegisterAction { get; set; }

		[JsonProperty("activateUser", Required = Required.Always)]
		public AppAction? ActivateUserAction { get; set; }

		[JsonProperty("userPage", Required = Required.AllowNull)]
		public NamedAction? UserPageAction { get; set; }
	}
}
