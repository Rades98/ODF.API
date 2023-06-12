using System.Text.Json.Serialization;
using ODF.API.Attributes.Binding;

namespace ODF.API.RequestModels
{
	public class BaseRequestModel
	{
		[JsonIgnore]
		[Country]
		public string CountryCode { get; set; } = string.Empty;
	}
}
