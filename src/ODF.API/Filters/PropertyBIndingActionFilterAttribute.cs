using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using ODF.API.Attributes.Binding;

namespace ODF.API.Filters
{
	public class PropertyBindingActionFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			foreach (var modelValuePair in context.ActionArguments.ToArray())
			{
				string parameterName = modelValuePair.Key;
				object? parameterValue = modelValuePair.Value;

				if (context.ActionArguments.ContainsKey(parameterName))
				{
					object? parameterBinding = context.ActionArguments[parameterName];

					if (parameterBinding is not null)
					{
						var parameterType = parameterBinding.GetType();

						foreach (var property in parameterType.GetProperties())
						{
							var binderAttributes = property.GetCustomAttributes<BindingAttribute>(true).ToList();
							binderAttributes.FirstOrDefault()?.Bind(property, parameterValue!);
						}
					}
				}
			}
		}
	}
}
