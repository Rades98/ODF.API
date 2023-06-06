using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;

namespace ODF.API.Controllers
{
	public class AboutController : BaseController
	{
		public AboutController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp) : base(mediator, apiSettings, adcp)
		{
		}

		[HttpGet(Name = nameof(GetAbout))]
		[ProducesResponseType(typeof(AboutResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAbout([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			string aboutText = await Mediator.Send(new GetTranslationQuery("Metropole Moravskoslezského kraje a její přilehlé okolí se mohou v listopadu těšit na 1. ročník festivalu Ostravské folklorní dny, které organizuje spolek FolklorOva. Akce, která se bude v centru Ostravy a městských částech konat od středy 8. do neděle 12. listopadu, má obyvatelům představit tradiční lidovou kulturu.", "about_info", countryCode), cancellationToken);
			string header = await Mediator.Send(new GetTranslationQuery("Ostravo, těš se na Ostravské dny folkloru!", "about_header", countryCode), cancellationToken);

			var responseModel = new AboutResponseModel(aboutText, header);

			responseModel.AddAction(GetQueriedAppAction(nameof(ArticlesController.GetArticles), "about_articles",
				new Dictionary<string, string> {
					{ "size", "10" },
					{ "offset", "0" },
					{ "pageId", "0" } }));

			return Ok(responseModel);
		}
	}
}
