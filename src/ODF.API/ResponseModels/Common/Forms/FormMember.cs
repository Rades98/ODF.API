namespace ODF.API.ResponseModels.Common.Forms
{
	public class FormMember
	{
		public FormMember(string name, string propName, string type, object? value, bool isEditable = false)
		{
			Name = name;
			PropName = propName;
			Type = type;
			PropValue = value;
			IsEditable = isEditable;
		}

		public string Name { get; }

		public string PropName { get; set; }

		public string Type { get; }

		public object? PropValue { get; }

		public bool IsEditable { get; }
	}
}
