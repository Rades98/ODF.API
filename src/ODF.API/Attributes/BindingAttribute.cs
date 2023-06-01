using System.Reflection;

namespace ODF.API.Attributes
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
	public abstract class BindingAttribute : Attribute
	{
		public virtual void Bind(PropertyInfo propInfo, object obj) { }
	}
}
