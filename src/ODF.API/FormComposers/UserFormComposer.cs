using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms.User;
using ODF.API.ResponseModels.Common.Forms;
using ODF.AppLayer.CQRS.Interfaces.User;
using ODF.AppLayer.Dtos;
using ODF.AppLayer.Extensions;
using ODF.Domain.Constants;
using ODF.Domain.Extensions;

namespace ODF.API.FormComposers
{
	public static class UserFormComposer
	{
		public static Form GetLoginForm(
			LoginUserForm userRequestForm,
			IReadOnlyList<TranslationDto> translations,
			IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();

			form.AddMember(new(translations.Get("login_username"), nameof(ILoginUser.UserName).ToCamelCase(), FormValueTypes.Text,
				userRequestForm.UserName ?? "", true, errors?.GetErrorMessage(nameof(ILoginUser.UserName))));

			form.AddMember(new(translations.Get("login_pw"), nameof(ILoginUser.Password).ToCamelCase(), FormValueTypes.Password,
				userRequestForm.Password ?? "", true, errors?.GetErrorMessage(nameof(ILoginUser.Password))));

			return form;
		}

		public static Form GetRegisterForm(
			RegisterUserForm userForm,
			IReadOnlyList<TranslationDto> translations,
			IEnumerable<ValidationFailure>? errors = null)
		{
			var form = new Form();
			form.AddMember(new(translations.Get("login_username"), nameof(IRegisterUser.UserName), FormValueTypes.Text,
				userForm.UserName, true, errors?.GetErrorMessage(nameof(IRegisterUser.UserName))));

			form.AddMember(new(translations.Get("login_pw"), nameof(IRegisterUser.Password), FormValueTypes.Password,
				userForm.Password, true, errors?.GetErrorMessage(nameof(IRegisterUser.Password))));

			form.AddMember(new(translations.Get("login_pw2"), nameof(IRegisterUser.Password2), FormValueTypes.Password,
				userForm.Password2, true, errors?.GetErrorMessage(nameof(IRegisterUser.Password2))));

			form.AddMember(new(translations.Get("login_email"), nameof(IRegisterUser.Email), FormValueTypes.Text,
				userForm.Email, true, errors?.GetErrorMessage(nameof(IRegisterUser.Email))));

			form.AddMember(new(translations.Get("login_first_name"), nameof(IRegisterUser.FirstName), FormValueTypes.Text,
				userForm.FirstName, true, errors?.GetErrorMessage(nameof(IRegisterUser.FirstName))));

			form.AddMember(new(translations.Get("login_last_name"), nameof(IRegisterUser.LastName), FormValueTypes.Text,
				userForm.LastName, true, errors?.GetErrorMessage(nameof(IRegisterUser.LastName))));

			return form;
		}

		public static Form GetActivateUserForm(ActivateUserForm form, IEnumerable<ValidationFailure>? errors = null)
		{
			var actForm = new Form();

			actForm.AddMember(new(nameof(IActivateUser.Hash), nameof(IActivateUser.Hash), FormValueTypes.Text,
				form.Hash, true, errors?.GetErrorMessage(nameof(IActivateUser.Hash))));

			return actForm;
		}
	}
}
