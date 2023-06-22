using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Lineup
{
	public class UpdateLineupResponseModel : BaseCreateResponseModel
	{
		public UpdateLineupResponseModel(string message) : base(message)
		{
		}

		public UpdateLineupResponseModel(Form form, string message) : base(form, message)
		{
		}

		public UpdateLineupResponseModel(Form form) : base(form)
		{
		}
	}
}
