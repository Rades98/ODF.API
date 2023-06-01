using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;

namespace ODF.API.Controllers
{
	public class AboutController : Controller
	{
		private readonly IMediator _mediator;
		private readonly ApiSettings _settings;

		public AboutController(IMediator mediator, IOptions<ApiSettings> apiSettings)
		{
			_mediator = mediator;
			_settings = apiSettings.Value;
		}


		[HttpGet("/{countryCode}/about")]
		[ProducesResponseType(typeof(AboutResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAbout([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			string aboutText = await _mediator.Send(new GetTranslationQuery("Metropole Moravskoslezského kraje a její přilehlé okolí se mohou v listopadu těšit na 1. ročník festivalu Ostravské folklorní dny, které organizuje spolek FolklorOva. Akce, která se bude v centru Ostravy a městských částech konat od středy 8. do neděle 12. listopadu, má obyvatelům představit tradiční lidovou kulturu.", "about_info", countryCode), cancellationToken);
			string header = await _mediator.Send(new GetTranslationQuery("Ostravo, těš se na Ostravské dny folkloru!", "about_header", countryCode), cancellationToken);

			var responseModel = new AboutResponseModel(_settings.ApiUrl, aboutText, header, countryCode);
			responseModel.AddAction($"/{countryCode}/articles?size=10&offset=0&pageId=0", "about_articles", HttpMethods.Get);

			return Ok(responseModel);
		}
	}
}
