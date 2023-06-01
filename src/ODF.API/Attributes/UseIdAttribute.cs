using System.Reflection;
using ODF.API.Extensions;
using ODF.API.Registration;

namespace ODF.API.Attributes
{
	/// <summary>
	/// User id attribute is bindable to nullable Guid
	/// </summary
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
