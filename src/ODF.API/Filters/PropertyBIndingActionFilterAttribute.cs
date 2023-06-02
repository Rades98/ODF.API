using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;
using ODF.API.Attributes;

namespace ODF.API.Filters
{
	public class PropertyBIndingActionFilterAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext actionContext)
		{
			foreach (var modelValuePair in actionContext.ActionArguments.ToArray())
			{
				string parameterName = modelValuePair.Key;
				object? parameterValue = modelValuePair.Value;

				if (actionContext.ActionArguments.ContainsKey(parameterName))
				{
					object? parameterBinding = actionContext.ActionArguments[parameterName];

					if (parameterBinding is not null)
					{
						var parameterType = parameterBinding.GetType();

						foreach (var property in parameterType.GetProperties())
						{
							var binderAttributes = property.GetCustomAttributes<BindingAttribute>(true).ToList();
							binderAttributes.FirstOrDefault()?.Bind(property, parameterValue);
						}
					}
				}
			}
		}
	}
}
