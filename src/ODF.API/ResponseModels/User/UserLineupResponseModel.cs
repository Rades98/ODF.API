using Newtonsoft.Json;
using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.User
{
	public class UserLineupResponseModel : BaseResponseModel
	{
		public UserLineupResponseModel() : base()
		{
		}


		[JsonProperty("lineup", Required = Required.Always)]
		public IDictionary<string, IEnumerable<UserLineupItemResponseModel>> Lineup { get; set; } = new Dictionary<string, IEnumerable<UserLineupItemResponseModel>>();
	}
}
