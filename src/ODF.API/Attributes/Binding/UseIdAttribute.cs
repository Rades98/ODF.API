using System.Reflection;
using ODF.API.Extensions;

namespace ODF.API.Attributes.Binding
{
	/// <summary>
	/// User id attribute is bindable to nullable Guid
	/// </summary
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
	public class UseIdAttribute : BindingAttribute
	{
		public override void Bind(PropertyInfo propInfo, object obj)
			=> propInfo.SetValue(obj, HttpContextAccessor.HttpContext!.GetUserId(), null);
	}
}
