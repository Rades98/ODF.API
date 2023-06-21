using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Lineup
{
	public class UpdateLineupResponseModel : BaseCreateResponseModel
	{
		public UpdateLineupResponseModel(string? message = null) : base(message)
		{
		}

		public UpdateLineupResponseModel(Form form, string? message = null) : base(form, message)
		{
		}
	}
}
