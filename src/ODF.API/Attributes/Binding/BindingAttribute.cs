using System.Reflection;
using ODF.API.Registration;

namespace ODF.API.Attributes.Binding
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
	public abstract class BindingAttribute : Attribute
	{
		public readonly IHttpContextAccessor HttpContextAccessor = ServiceLocator.Instance!.GetService<IHttpContextAccessor>()!;
		public virtual void Bind(PropertyInfo propInfo, object obj) { }
	}
}
