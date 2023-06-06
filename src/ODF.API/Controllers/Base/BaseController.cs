using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Extensions;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;

namespace ODF.API.Controllers.Base
{
	[ApiController]
	[Route("/{countryCode}/[controller]")]
	public abstract class BaseController : Controller
	{
		private readonly IMediator _mediator;
		private readonly ApiSettings _settings;
		private readonly string _baseUrl;
		private readonly IActionDescriptorCollectionProvider _adcp;

		public IMediator Mediator => _mediator;

		public string ApiBaseUrl => _baseUrl;

		protected BaseController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_settings = apiSettings.Value ?? throw new ArgumentNullException(nameof(apiSettings));
			_adcp = adcp ?? throw new ArgumentNullException(nameof(adcp));

			_baseUrl = _settings.ApiUrl;
		}

		internal NamedAction GetNamedAction(string actionIdentifier, string actionName, string rel, Form? actionForm = null, string? countryCode = null)
		{
			var appAction = GetAppAction(actionIdentifier, rel, countryCode: countryCode);
			return new(appAction.Curl.Href.ToString()!, actionName, appAction.Curl.Rel, appAction.Curl.Method, form: actionForm);
		}

		internal AppAction GetAppAction(string actionIdentifier, string rel, Form? actionForm = null, string? countryCode = null)
		{
			var action = _adcp.ActionDescriptors.Items.FirstOrDefault(act => act.RouteValues["action"] == actionIdentifier);

			if (action is null)
			{
				throw new NullReferenceException(nameof(action));
			}

			string method = HttpMethods.Put;

			if (action.ActionConstraints!.FirstOrDefault(constraint => constraint is HttpMethodActionConstraint httpMethodConstraint) is HttpMethodActionConstraint httpMethodConstraint)
			{
				method = httpMethodConstraint.HttpMethods.First();
			}

			var link = action.AttributeRouteInfo!.Template!.Replace("{countryCode}", countryCode ?? HttpContext.GetCountryCode()!);

			return new($"{ApiBaseUrl}/{link}", rel, ((HttpMethodActionConstraint)action.ActionConstraints!.First()).HttpMethods.First(), form: actionForm);
		}

		internal AppAction GetQueriedAppAction(string actionIdentifier, string rel, Dictionary<string, string> queryParams, Form? actionForm = null)
		{
			var action = _adcp.ActionDescriptors.Items.FirstOrDefault(act => act.RouteValues["action"] == actionIdentifier);

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
				.Replace("{countryCode}", HttpContext.GetCountryCode()!);

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

			return new($"{ApiBaseUrl}/{link}", rel, ((HttpMethodActionConstraint)action.ActionConstraints!.First()).HttpMethods.First(), form: actionForm);
		}

		internal string GetRelativeUrlByAction(string actionIdentifier)
		{
			var action = _adcp.ActionDescriptors.Items.FirstOrDefault(act => act.RouteValues["action"] == actionIdentifier);

			if (action is null)
			{
				throw new NullReferenceException(nameof(action));
			}

			var link = action.AttributeRouteInfo!.Template!
				.Replace("{countryCode}", HttpContext.GetCountryCode()!);

			return link;
		}
	}
}
