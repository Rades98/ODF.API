using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.FormFactories
{
	public static class LineupItemFormFactory
	{
		public static Form GetAddLineupItemForm(string place, string interpret, string performanceName, string description, string DescriptionTranslationCode, DateTime dateTime)
		{
			var form = new Form();
			form.AddMember(new("Místo", nameof(place), "text", place, true));
			form.AddMember(new("Interpret", nameof(interpret), "text", interpret, true));
			form.AddMember(new("Název vystoupení", nameof(performanceName), "text", performanceName, true));
			form.AddMember(new("Popis", nameof(description), "text", description, true));
			form.AddMember(new("Překladová proměnná popisu", nameof(DescriptionTranslationCode), "text", DescriptionTranslationCode, true));
			form.AddMember(new("Datum a čas", nameof(dateTime), "datetime-local", dateTime, true));

			return form;
		}
	}
}
