using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Lineup
{
	public class AddLineupResponseModel : BaseCreateResponseModel
	{
		public AddLineupResponseModel(string message) : base(message)
		{
		}

		public AddLineupResponseModel(Form form, string message) : base(form, message)
		{
		}

		public AddLineupResponseModel(Form form) : base(form)
		{
		}
	}
}
