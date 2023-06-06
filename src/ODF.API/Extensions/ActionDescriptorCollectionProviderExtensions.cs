using System.Text;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.Extensions
{
	public static class ActionDescriptorCollectionProviderExtensions
	{
		internal static NamedAction GetNamedAction(
			this IActionDescriptorCollectionProvider adcp, HttpContext context, string baseUrl,
			string actionIdentifier, string actionName, string rel, Form? actionForm = null, string? countryCode = null)
		{
			var appAction = adcp.GetAppAction(context, baseUrl, actionIdentifier, rel, countryCode: countryCode);
			return new(appAction.Curl.Href.ToString()!, actionName, appAction.Curl.Rel, appAction.Curl.Method, form: actionForm);
		}

		internal static AppAction GetAppAction(
			this IActionDescriptorCollectionProvider adcp, HttpContext context, string baseUrl,
			string actionIdentifier, string rel, Form? actionForm = null, string? countryCode = null)
		{
			var action = adcp.ActionDescriptors.Items.FirstOrDefault(act => act.RouteValues["action"] == actionIdentifier);

			if (action is null)
			{
				throw new NullReferenceException(nameof(action));
			}

			string method = HttpMethods.Put;

			if (action.ActionConstraints!.FirstOrDefault(constraint => constraint is HttpMethodActionConstraint httpMethodConstraint) is HttpMethodActionConstraint httpMethodConstraint)
			{
				method = httpMethodConstraint.HttpMethods.First();
			}

			var link = action.AttributeRouteInfo!.Template!.Replace("{countryCode}", countryCode ?? context.GetCountryCode()!);

			return new($"{baseUrl}/{link}", rel, ((HttpMethodActionConstraint)action.ActionConstraints!.First()).HttpMethods.First(), form: actionForm);
		}

		internal static AppAction GetQueriedAppAction(
			this IActionDescriptorCollectionProvider adcp, HttpContext context, string baseUrl,
			string actionIdentifier, string rel, Dictionary<string, string> queryParams, Form? actionForm = null)
		{
			var action = adcp.ActionDescriptors.Items.FirstOrDefault(act => act.RouteValues["action"] == actionIdentifier);

			if (action is null)
			{
				throw new NullReferenceException(nameof(action));
			}

			string method = HttpMethods.Put;

			if (action.ActionConstraints!.FirstOrDefault(constraint => constraint is HttpMethodActionConstraint httpMethodConstraint) is HttpMethodActionConstraint httpMethodConstraint)
			{
				method = httpMethodConstraint.HttpMethods.First();
			}

			var link = action.AttributeRouteInfo!.Template!
				.Replace("{countryCode}", context.GetCountryCode()!);

			var linkAddition = new StringBuilder("");
			foreach (ParameterDescriptor param in action.Parameters)
			{
				if (queryParams.Keys.Contains(param.Name))
				{
					linkAddition.Append($"&{param.Name}={queryParams[param.Name]}");
				}
			}

			if (linkAddition.Length > 0)
			{
				link += string.Concat("?", linkAddition.ToString().AsSpan(1));
			}

			return new($"{baseUrl}/{link}", rel, ((HttpMethodActionConstraint)action.ActionConstraints!.First()).HttpMethods.First(), form: actionForm);
		}

		internal static string GetRelativeUrlByAction(this IActionDescriptorCollectionProvider adcp, HttpContext context, string actionIdentifier)
		{
			var action = adcp.ActionDescriptors.Items.FirstOrDefault(act => act.RouteValues["action"] == actionIdentifier);

			if (action is null)
			{
				throw new NullReferenceException(nameof(action));
			}

			var link = action.AttributeRouteInfo!.Template!
				.Replace("{countryCode}", context.GetCountryCode()!);

			return link;
		}
	}
}
