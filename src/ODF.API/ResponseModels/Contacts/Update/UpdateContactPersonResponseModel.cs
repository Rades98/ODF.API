using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.Contacts.Update
{
    public class UpdateContactPersonResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactPersonResponseModel() : base(HttpMethods.Post)
		{
		}
	}
}
