using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Association;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;

namespace ODF.API.Controllers
{
    public class AssociationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApiSettings _settings;

        public AssociationsController(IMediator mediator, IOptions<ApiSettings> apiSettings)
        {
            _mediator = mediator;
            _settings = apiSettings.Value;
        }


        [HttpGet("/{countryCode}/associations")]
        [ProducesResponseType(typeof(AssociationResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAssociation([FromRoute] string countryCode)
        {
            var aboutText = await _mediator.Send(new GetTranslationQuery("Jsme FolklorOVA, spolek nadšenců, kteří chtějí podporovat a dále rozvíjet kulturu v Ostravě a jejím okolí. Skrz akci Ostravské dny folkloru chceme Ostravanům ukázat tradiční lidovou kulturu a věříme, že je nadchne stejně jako nás. Lidová kultura a folklor nezná hranic je tu pro všechny, malé i velké, stejně jako pro staré i mladé.\nFolklor spojuje!", "association_info", countryCode));
            var header = await _mediator.Send(new GetTranslationQuery("O nás", "association_header", countryCode));
            var responseModel = new AssociationResponseModel(_settings.ApiUrl, aboutText, header, countryCode);

            responseModel.AddAction($"/{countryCode}/articles?size=10&offset=0&pageId=1", "association_articles", HttpMethods.Get);

            return Ok(responseModel);
        }
    }
}
