using Newtonsoft.Json;
using ODF.API.Attributes.Binding;

namespace ODF.API.RequestModels.Navigation
{
	public class NavigationRequestModel : BaseRequestModel
	{
		[JsonIgnore]
		[IsAdmin]
		public bool IsAdmin { get; set; }

		[JsonIgnore]
		[IsLoggedIn]
		public bool IsLoggedIn { get; set; }
	}
}
