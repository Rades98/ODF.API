using System.Text;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.Extensions
{
	public static class ActionDescriptorCollectionProviderExtensions
	{
		internal static NamedAction GetNamedAction(
			this IActionDescriptorCollectionProvider adcp, string baseUrl,
			string actionIdentifier, string actionName, string rel, Form? actionForm = null)
		{
			var appAction = adcp.GetAppAction(baseUrl, actionIdentifier, rel);
			return new(appAction.Curl.Href.ToString()!, actionName, appAction.Curl.Rel, appAction.Curl.Method, form: actionForm);
		}

		internal static AppAction GetAppAction(
			this IActionDescriptorCollectionProvider adcp, string baseUrl,
			string actionIdentifier, string rel, Form? actionForm = null)
		{
			var action = adcp.ActionDescriptors.Items.FirstOrDefault(act => act.RouteValues["action"] == actionIdentifier);

			if (action is null)
			{
				throw new ArgumentException(nameof(action));
			}

			string link = action.AttributeRouteInfo!.Template!;

			return new($"{baseUrl}/{link}", rel, ((HttpMethodActionConstraint)action.ActionConstraints![0]).HttpMethods.First(), form: actionForm);
		}

		internal static AppAction GetQueriedAppAction(
			this IActionDescriptorCollectionProvider adcp, string baseUrl,
			string actionIdentifier, string rel, Dictionary<string, string> queryParams, Form? actionForm = null)
		{
			var action = adcp.ActionDescriptors.Items.FirstOrDefault(act => act.RouteValues["action"] == actionIdentifier);

			if (action is null)
			{
				throw new ArgumentException(nameof(action));
			}

			string link = action.AttributeRouteInfo!.Template!;

			string linkAddition = action.Parameters
											.Where(param => queryParams.ContainsKey(param.Name))
											.Aggregate(new StringBuilder(), (sb, param) => sb.Append($"&{param.Name}={queryParams[param.Name]}"))
											.ToString();

			if (linkAddition.Length > 0)
			{
				link += string.Concat("?", linkAddition.ToString().AsSpan(1));
			}

			return new($"{baseUrl}/{link}", rel, ((HttpMethodActionConstraint)action.ActionConstraints![0]).HttpMethods.First(), form: actionForm);
		}

		internal static string GetRelativeUrlByAction(this IActionDescriptorCollectionProvider adcp, string actionIdentifier)
		{
			var action = adcp.ActionDescriptors.Items.FirstOrDefault(act => act.RouteValues["action"] == actionIdentifier);

			if (action is null)
			{
				throw new ArgumentException(nameof(action));
			}

			return action.AttributeRouteInfo!.Template!;
		}
	}
}
