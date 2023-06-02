using System.Reflection;
using ODF.API.Extensions;
using ODF.API.Registration;

namespace ODF.API.Attributes
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
	public class IsLoggedInAttribute : BindingAttribute
	{
		public override void Bind(PropertyInfo propInfo, object obj)
		{
			var httpContextAccessor = ServiceLocator.Instance!.GetService<IHttpContextAccessor>()!;
			bool? isAdmin = httpContextAccessor.HttpContext!.IsLoggedIn();
			if (isAdmin.HasValue)
			{
				propInfo.SetValue(obj, isAdmin, null);
			}
		}
	}
}
