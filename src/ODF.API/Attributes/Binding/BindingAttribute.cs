using System.Reflection;

namespace ODF.API.Attributes.Binding
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
    public abstract class BindingAttribute : Attribute
    {
        public virtual void Bind(PropertyInfo propInfo, object obj) { }
    }
}
