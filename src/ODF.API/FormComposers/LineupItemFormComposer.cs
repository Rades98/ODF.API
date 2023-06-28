using FluentValidation.Results;
using ODF.API.Extensions;
using ODF.API.RequestModels.Forms.Lineup;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;
using ODF.AppLayer.CQRS.Interfaces.Lineup;
using ODF.Domain.Constants;

namespace ODF.API.FormComposers
{
	public static class LineupItemFormComposer
	{
		public static Form GetAddLineupItemForm(AddLineupItemForm form, IEnumerable<ValidationFailure>? errors = null, AppAction? usersDataSource = null)
		{
			var requestForm = new Form();
			requestForm.AddMember(new("Místo", nameof(IAddLineupItem.Place), FormValueTypes.Text, form.Place, true,
				errors?.GetErrorMessage(nameof(IAddLineupItem.Place))));

			requestForm.AddMember(new("Interpret", nameof(IAddLineupItem.Interpret), FormValueTypes.Text, form.Interpret, true,
				errors?.GetErrorMessage(nameof(IAddLineupItem.Interpret))));

			requestForm.AddMember(new("Název vystoupení", nameof(IAddLineupItem.PerformanceName), FormValueTypes.Text, form.PerformanceName, true,
				errors?.GetErrorMessage(nameof(IAddLineupItem.PerformanceName))));

			requestForm.AddMember(new("Popis", nameof(IAddLineupItem.Description), FormValueTypes.Text, form.Description, true,
				errors?.GetErrorMessage(nameof(IAddLineupItem.Description))));

			requestForm.AddMember(new("Překladová proměnná popisu", nameof(IAddLineupItem.DescriptionTranslationCode), FormValueTypes.Text, form.DescriptionTranslationCode, true,
				errors?.GetErrorMessage(nameof(IAddLineupItem.DescriptionTranslationCode))));

			requestForm.AddMember(new("Datum a čas", nameof(IAddLineupItem.DateTime), FormValueTypes.DateTimeLocal, form.DateTime, true,
				errors?.GetErrorMessage(nameof(IAddLineupItem.DateTime))));

			requestForm.AddMember(new("Jméno uživatele", nameof(IAddLineupItem.UserName), FormValueTypes.Text, form.UserName, true,
				errors?.GetErrorMessage(nameof(IAddLineupItem.UserName)), usersDataSource));

			requestForm.AddMember(new("Poznámka pro uživatele", nameof(IAddLineupItem.UserNote), FormValueTypes.Text, form.UserName, true,
				errors?.GetErrorMessage(nameof(IAddLineupItem.UserNote)), usersDataSource));

			return requestForm;
		}

		public static Form GetDeleteLineupItemForm(DeleteLineupItemForm form, IEnumerable<ValidationFailure>? errors = null)
		{
			var requestForm = new Form();

			requestForm.AddMember(new("Id položky", nameof(IDeleteLineupItem.Id), FormValueTypes.Text, form.Id, false,
				errors?.GetErrorMessage(nameof(IDeleteLineupItem.Id))));

			return requestForm;
		}

		public static Form GetUdpateLineupItemForm(UpdateLineupItemForm form, IEnumerable<ValidationFailure>? errors = null, AppAction? usersDataSource = null)
		{
			var requestForm = new Form();

			requestForm.AddMember(new("Id položky", nameof(IUpdateLineupItem.Id), FormValueTypes.Text, form.Id, false,
				errors?.GetErrorMessage(nameof(IUpdateLineupItem.Id))));

			requestForm.AddMember(new("Místo", nameof(IUpdateLineupItem.Place), FormValueTypes.Text, form.Place, true,
				errors?.GetErrorMessage(nameof(IUpdateLineupItem.Place))));

			requestForm.AddMember(new("Interpret", nameof(IUpdateLineupItem.Interpret), FormValueTypes.Text, form.Interpret, true,
				errors?.GetErrorMessage(nameof(IUpdateLineupItem.Interpret))));

			requestForm.AddMember(new("Název vystoupení", nameof(IUpdateLineupItem.PerformanceName), FormValueTypes.Text, form.PerformanceName, true,
				errors?.GetErrorMessage(nameof(IUpdateLineupItem.PerformanceName))));

			requestForm.AddMember(new("Popis", nameof(IUpdateLineupItem.Description), FormValueTypes.Text, form.Description, true,
				errors?.GetErrorMessage(nameof(IUpdateLineupItem.Description))));

			requestForm.AddMember(new("Překladová proměnná popisu", nameof(IUpdateLineupItem.DescriptionTranslationCode), FormValueTypes.Text, form.DescriptionTranslationCode, false,
				errors?.GetErrorMessage(nameof(IUpdateLineupItem.DescriptionTranslationCode))));

			requestForm.AddMember(new("Datum a čas", nameof(IUpdateLineupItem.DateTime), FormValueTypes.DateTimeLocal, form.DateTime, true,
				errors?.GetErrorMessage(nameof(IUpdateLineupItem.DateTime))));

			requestForm.AddMember(new("Jméno uživatele", nameof(IUpdateLineupItem.UserName), FormValueTypes.Text, form.UserName, true,
				errors?.GetErrorMessage(nameof(IUpdateLineupItem.UserName)), usersDataSource));

			requestForm.AddMember(new("Poznámka pro uživatele", nameof(IUpdateLineupItem.UserNote), FormValueTypes.Text, form.UserName, true,
				errors?.GetErrorMessage(nameof(IUpdateLineupItem.UserNote)), usersDataSource));

			return requestForm;
		}
	}
}
