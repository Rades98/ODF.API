using ODF.API.ResponseModels.Base;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.ResponseModels.LanguageMutations
{
	public class UpdateTranslationResponseModel : BaseUpdateResponseModel
	{
		public UpdateTranslationResponseModel(Form form, string message) : base(form, message)
		{
		}

		public UpdateTranslationResponseModel(Form form) : base(form)
		{
		}
	}
}
