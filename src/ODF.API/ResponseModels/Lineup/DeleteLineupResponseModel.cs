using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.Lineup
{
	public class DeleteLineupResponseModel : BaseCreateResponseModel
	{
		public DeleteLineupResponseModel(string? message = null, Form? form = null) : base(message, form)
		{
		}
	}
}
