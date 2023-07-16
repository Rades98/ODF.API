using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Extensions;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Common.Forms;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.Responses;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers.Base
{
	[ApiController]
	[Route("/api/{countryCode}/[controller]")]
	public abstract class BaseController : Controller
	{
		private readonly IMediator _mediator;
		private readonly string _baseUrl;
		private readonly string _feUrl;
		private readonly Uri _signalHubUrl;
		private readonly IActionDescriptorCollectionProvider _adcp;
		private readonly ITranslationsProvider _translationsProvider;

		public IMediator Mediator => _mediator;

		public string ApiBaseUrl => _baseUrl;
		public string FrontEndUrl => _feUrl;

		public Uri SignalHubUrl => _signalHubUrl;

		public ITranslationsProvider TranslationsProvider => _translationsProvider;

		protected BaseController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_ = apiSettings.Value ?? throw new ArgumentNullException(nameof(apiSettings));
			_adcp = adcp ?? throw new ArgumentNullException(nameof(adcp));
			_translationsProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));

			_baseUrl = apiSettings.Value.ApiUrl;
			_feUrl = apiSettings.Value.FrontEndUrl;
			_signalHubUrl = new(_baseUrl + apiSettings.Value.SignalChatHubPath);
		}

		internal NamedAction GetNamedAction(string actionIdentifier, string actionName, string rel, Form? actionForm = null, string? countryCode = null)
			=> _adcp.GetNamedAction(HttpContext, ApiBaseUrl, actionIdentifier, actionName, rel, actionForm, countryCode);

		internal AppAction GetAppAction(string actionIdentifier, string rel, Form? actionForm = null, string? countryCode = null)
			=> _adcp.GetAppAction(HttpContext, ApiBaseUrl, actionIdentifier, rel, actionForm, countryCode);

		internal AppAction GetQueriedAppAction(string actionIdentifier, string rel, Dictionary<string, string> queryParams, Form? actionForm = null)
		 => _adcp.GetQueriedAppAction(HttpContext, ApiBaseUrl, actionIdentifier, rel, queryParams, actionForm);

		internal string GetRelativeUrlByAction(string actionIdentifier)
			=> _adcp.GetRelativeUrlByAction(HttpContext, actionIdentifier);

		internal static IActionResult InternalServerError(ExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.InternalServerError, response);

		internal static IActionResult NotFound(NotFoundExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.NotFound, response);

		internal static IActionResult BadRequest(BadRequestExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.BadRequest, response);

		internal static IActionResult Unauthorized(UnauthorizedExceptionResponseModel response)
			=> new ApiResult(System.Net.HttpStatusCode.Unauthorized, response);
	}
}
