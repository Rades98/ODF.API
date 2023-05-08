using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.FormFactories
{
	public static class UserFormFactory
	{
		public static Form GetLoginForm(string userNameTranslation, string passwordTranslation, string userName = "", string password = "")
		{
			var form = new Form();
			form.AddMember(new(userNameTranslation, nameof(userName), "text", userName, true));
			form.AddMember(new(passwordTranslation, nameof(password), "password", password, true));

			return form;
		}

		public static Form GetRegisterForm(
			string userNameTranslation, 
			string passwordTranslation, 
			string password2Translation, 
			string emailTranslation,
			string firstNameTranslation,
			string lastNameTranslation,
			string userName = "", 
			string password = "", 
			string password2 = "", 
			string email = "", 
			string firstName = "", 
			string lastName ="")
		{
			var form = new Form();
			form.AddMember(new(userNameTranslation, nameof(userName), "text", userName, true));
			form.AddMember(new(passwordTranslation, nameof(password), "password", password, true));
			form.AddMember(new(password2Translation, nameof(password2), "password", password2, true));
			form.AddMember(new(emailTranslation, nameof(email), "text", email, true));
			form.AddMember(new(firstNameTranslation, nameof(firstName), "text", firstName, true));
			form.AddMember(new(lastNameTranslation, nameof(lastName), "text", lastName, true));

			return form;
		}
	}
}
