using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common.Forms;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Extensions;

namespace ODF.API.FormFactories
{
	public static class UserFormFactory
	{
		public static Form GetLoginForm(
			IReadOnlyList<TranslationDto> translations,
			UserRequestForm? userRequestForm = null,
			IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();

			var pwError = errors?.FirstOrDefault(p => p.PropertyName == nameof(UserRequestForm.UserName))?.ErrorMessage;

			form.AddMember(new(translations.Get("login_username"), nameof(UserRequestForm.UserName).ToCamelCase(),
				"text", userRequestForm?.UserName ?? "", true, errors?.FirstOrDefault(p => p.PropertyName == nameof(UserRequestForm.UserName))?.ErrorMessage));

			form.AddMember(new(translations.Get("login_pw"), nameof(UserRequestForm.Password).ToCamelCase(),
				"password", userRequestForm?.Password ?? "", true, pwError is not null ? translations.Get(pwError) : null));

			return form;
		}

		public static Form GetRegisterForm(
			IReadOnlyList<TranslationDto> translations,
			string userName = "",
			string password = "",
			string password2 = "",
			string email = "",
			string firstName = "",
			string lastName = "")
		{
			var form = new Form();
			form.AddMember(new(translations.Get("login_username"), nameof(userName), "text", userName, true));
			form.AddMember(new(translations.Get("login_pw"), nameof(password), "password", password, true));
			form.AddMember(new(translations.Get("login_pw2"), nameof(password2), "password", password2, true));
			form.AddMember(new(translations.Get("login_email"), nameof(email), "text", email, true));
			form.AddMember(new(translations.Get("login_first_name"), nameof(firstName), "text", firstName, true));
			form.AddMember(new(translations.Get("login_last_name"), nameof(lastName), "text", lastName, true));

			return form;
		}
	}
}
