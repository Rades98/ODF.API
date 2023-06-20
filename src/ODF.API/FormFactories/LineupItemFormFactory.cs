using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.Common.Forms;
using ODF.Domain.Constants;

namespace ODF.API.FormFactories
{
	public static class LineupItemFormFactory
	{
		public static Form GetAddLineupItemForm(AddLineupItemForm form, IEnumerable<ValidationFailure>? errors = null)
		{
			var requestForm = new Form();
			requestForm.AddMember(new("Místo", nameof(AddLineupItemForm.Place), FormValueTypes.Text, form.Place, true,
				errors?.GetErrorMessage(nameof(AddLineupItemForm.Place))));

			requestForm.AddMember(new("Interpret", nameof(AddLineupItemForm.Interpret), FormValueTypes.Text, form.Interpret, true,
				errors?.GetErrorMessage(nameof(AddLineupItemForm.Interpret))));

			requestForm.AddMember(new("Název vystoupení", nameof(AddLineupItemForm.PerformanceName), FormValueTypes.Text, form.PerformanceName, true,
				errors?.GetErrorMessage(nameof(AddLineupItemForm.PerformanceName))));

			requestForm.AddMember(new("Popis", nameof(AddLineupItemForm.Description), FormValueTypes.Text, form.Description, true,
				errors?.GetErrorMessage(nameof(AddLineupItemForm.Description))));

			requestForm.AddMember(new("Překladová proměnná popisu", nameof(AddLineupItemForm.DescriptionTranslationCode), FormValueTypes.Text, form.DescriptionTranslationCode, true,
				errors?.GetErrorMessage(nameof(AddLineupItemForm.DescriptionTranslationCode))));

			requestForm.AddMember(new("Datum a čas", nameof(AddLineupItemForm.DateTime), FormValueTypes.DateTimeLocal, form.DateTime, true,
				errors?.GetErrorMessage(nameof(AddLineupItemForm.DateTime))));

			requestForm.AddMember(new("Jméno uživatele", nameof(AddLineupItemForm.UserName), FormValueTypes.Text, form.UserName, true,
				errors?.GetErrorMessage(nameof(AddLineupItemForm.UserName))));

			return requestForm;
		}
	}
}
