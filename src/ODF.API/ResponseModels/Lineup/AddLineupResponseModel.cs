using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Lineup
{
	public class AddLineupResponseModel : BaseCreateResponseModel
	{
		public AddLineupResponseModel(string? message = null, Form? form = null) : base(message, form)
		{
		}
	}
}
