using System.Reflection;
using ODF.API.Extensions;
using ODF.API.Registration;

namespace ODF.API.Attributes.Binding
{
	/// <summary>
	/// User id attribute is bindable to nullable Guid
	/// </summary
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
	public class UseIdAttribute : BindingAttribute
	{
		public override void Bind(PropertyInfo propInfo, object obj)
		{
			var httpContextAccessor = ServiceLocator.Instance!.GetService<IHttpContextAccessor>()!;
			var userId = httpContextAccessor.HttpContext!.GetUserId();
			propInfo.SetValue(obj, userId, null);
		}
	}
}
