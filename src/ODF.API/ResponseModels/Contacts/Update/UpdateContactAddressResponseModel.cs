using ODF.API.ResponseModels.Base;

namespace ODF.API.ResponseModels.Contacts.Update
{
    public class UpdateContactAddressResponseModel : BaseUpdateResponseModel
	{
		public UpdateContactAddressResponseModel() : base(HttpMethods.Post)
		{
		}
	}
}
