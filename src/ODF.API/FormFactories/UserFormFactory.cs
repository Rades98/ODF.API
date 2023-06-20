using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms.User;
using ODF.API.ResponseModels.Common.Forms;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Extensions;
using ODF.Domain.Constants;

namespace ODF.API.FormFactories
{
	public static class UserFormFactory
	{
		public static Form GetLoginForm(
			LoginUserForm userRequestForm,
			IReadOnlyList<TranslationDto> translations,
			IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();

			form.AddMember(new(translations.Get("login_username"), nameof(LoginUserForm.UserName).ToCamelCase(), FormValueTypes.Text,
				userRequestForm.UserName ?? "", true, errors?.GetErrorMessage(nameof(LoginUserForm.UserName))));

			form.AddMember(new(translations.Get("login_pw"), nameof(LoginUserForm.Password).ToCamelCase(), FormValueTypes.Password,
				userRequestForm.Password ?? "", true, errors?.GetErrorMessage(nameof(LoginUserForm.Password))));

			return form;
		}

		public static Form GetRegisterForm(
			RegisterUserForm userForm,
			IReadOnlyList<TranslationDto> translations,
			IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new(translations.Get("login_username"), nameof(RegisterUserForm.UserName), FormValueTypes.Text,
				userForm.UserName, true, errors?.GetErrorMessage(nameof(RegisterUserForm.UserName))));

			form.AddMember(new(translations.Get("login_pw"), nameof(RegisterUserForm.Password), FormValueTypes.Password,
				userForm.Password, true, errors?.GetErrorMessage(nameof(RegisterUserForm.Password))));

			form.AddMember(new(translations.Get("login_pw2"), nameof(RegisterUserForm.Password2), FormValueTypes.Password,
				userForm.Password2, true, errors?.GetErrorMessage(nameof(RegisterUserForm.Password2))));

			form.AddMember(new(translations.Get("login_email"), nameof(RegisterUserForm.Email), FormValueTypes.Text,
				userForm.Email, true, errors?.GetErrorMessage(nameof(RegisterUserForm.Email))));

			form.AddMember(new(translations.Get("login_first_name"), nameof(RegisterUserForm.FirstName), FormValueTypes.Text,
				userForm.FirstName, true, errors?.GetErrorMessage(nameof(RegisterUserForm.FirstName))));

			form.AddMember(new(translations.Get("login_last_name"), nameof(RegisterUserForm.LastName), FormValueTypes.Text,
				userForm.LastName, true, errors?.GetErrorMessage(nameof(RegisterUserForm.LastName))));

			return form;
		}
	}
}
