using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.FormFactories
{
	public static class LineupItemFormFactory
	{
		public static Form GetAddLineupItemForm(string place, string interpret, string performanceName, string performanceNameTranslationCode, string description, string DescriptionTranslationCode, DateTime dateTime)
		{
			var form = new Form();
			form.AddMember(new("Místo", nameof(place), place.GetType().Name, place, true));
			form.AddMember(new("Interpret", nameof(interpret), interpret.GetType().Name, interpret, true));
			form.AddMember(new("Název vystoupení", nameof(performanceName), performanceName.GetType().Name, performanceName, true));
			form.AddMember(new("Překladová proměnná názvu", nameof(performanceNameTranslationCode), performanceNameTranslationCode.GetType().Name, performanceNameTranslationCode, true));
			form.AddMember(new("Popis", nameof(description), description.GetType().Name, description, true));
			form.AddMember(new("Překladová proměnná popisu", nameof(DescriptionTranslationCode), DescriptionTranslationCode.GetType().Name, DescriptionTranslationCode, true));
			form.AddMember(new("Datum a čas", nameof(dateTime), dateTime.GetType().Name, dateTime, true));

			return form;
		}
	}
}
