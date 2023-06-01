using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Association;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;

namespace ODF.API.Controllers
{
	public class AssociationsController : BaseController
	{
		public AssociationsController(IMediator mediator, IOptions<ApiSettings> apiSettings) : base(mediator, apiSettings)
		{
		}


		[HttpGet("/{countryCode}/associations")]
		[ProducesResponseType(typeof(AssociationResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAssociation([FromRoute] string countryCode)
		{
			string aboutText = await Mediator.Send(new GetTranslationQuery("Jsme FolklorOVA, spolek nadšenců, kteří chtějí podporovat a dále rozvíjet kulturu v Ostravě a jejím okolí. Skrz akci Ostravské dny folkloru chceme Ostravanům ukázat tradiční lidovou kulturu a věříme, že je nadchne stejně jako nás. Lidová kultura a folklor nezná hranic je tu pro všechny, malé i velké, stejně jako pro staré i mladé.\nFolklor spojuje!", "association_info", countryCode));
			string header = await Mediator.Send(new GetTranslationQuery("O nás", "association_header", countryCode));
			var responseModel = new AssociationResponseModel(ApiSettings.ApiUrl, aboutText, header, countryCode);

			responseModel.AddAction($"/{countryCode}/articles?size=10&offset=0&pageId=1", "association_articles", HttpMethods.Get);

			return Ok(responseModel);
		}
	}
}
