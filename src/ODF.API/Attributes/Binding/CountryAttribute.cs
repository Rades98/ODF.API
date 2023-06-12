using System.Reflection;
using ODF.API.Extensions;
using ODF.API.Registration;

namespace ODF.API.Attributes.Binding
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
	public class CountryAttribute : BindingAttribute
	{
		public override void Bind(PropertyInfo propInfo, object obj)
		{
			var httpContextAccessor = ServiceLocator.Instance!.GetService<IHttpContextAccessor>()!;
			string? countryCode = httpContextAccessor.HttpContext!.GetCountryCode();
			if (countryCode is not null)
			{
				propInfo.SetValue(obj, countryCode, null);
			}
		}
	}
}
