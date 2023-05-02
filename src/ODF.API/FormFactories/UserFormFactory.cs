using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.FormFactories
{
	public static class UserFormFactory
	{
		public static Form GetLoginForm(string userNameTranslation, string passwordTranslation, string userName = "", string password = "")
		{
			var form = new Form();
			form.AddMember(new(userNameTranslation, nameof(userName), userName.GetType().Name, userName, true));
			form.AddMember(new(passwordTranslation, nameof(password), password.GetType().Name, password, true));

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
			form.AddMember(new(userNameTranslation, nameof(userName), userName.GetType().Name, userName, true));
			form.AddMember(new(passwordTranslation, nameof(password), password.GetType().Name, password, true));
			form.AddMember(new(password2Translation, nameof(password2), password2.GetType().Name, password2, true));
			form.AddMember(new(emailTranslation, nameof(email), email.GetType().Name, email, true));
			form.AddMember(new(firstNameTranslation, nameof(firstName), firstName.GetType().Name, firstName, true));
			form.AddMember(new(lastNameTranslation, nameof(lastName), lastName.GetType().Name, lastName, true));

			return form;
		}
	}
}
