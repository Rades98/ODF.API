namespace ODF.API.ResponseModels.Common.Forms
{
	public class Form
	{
		private readonly ICollection<FormMember> _props = new List<FormMember>();

		public IEnumerable<FormMember> Props => _props;

		internal IEnumerable<FormMember> AddMember(FormMember member)
		{
			_props.Add(member);
			return _props;
		}
	}
}
