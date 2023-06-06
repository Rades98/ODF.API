using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Association;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.CQRS.Translations.Queries;

namespace ODF.API.Controllers
{
	public class AssociationController : BaseController
	{
		public AssociationController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp) : base(mediator, apiSettings, adcp)
		{
		}

		[HttpGet(Name = nameof(GetAssociation))]
		[ProducesResponseType(typeof(AssociationResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAssociation([FromRoute] string countryCode)
		{
			string aboutText = await Mediator.Send(new GetTranslationQuery("Jsme FolklorOVA, spolek nadšenců, kteří chtějí podporovat a dále rozvíjet kulturu v Ostravě a jejím okolí. Skrz akci Ostravské dny folkloru chceme Ostravanům ukázat tradiční lidovou kulturu a věříme, že je nadchne stejně jako nás. Lidová kultura a folklor nezná hranic je tu pro všechny, malé i velké, stejně jako pro staré i mladé.\nFolklor spojuje!", "association_info", countryCode));
			string header = await Mediator.Send(new GetTranslationQuery("O nás", "association_header", countryCode));
			var responseModel = new AssociationResponseModel(aboutText, header);

			responseModel.AddAction(GetQueriedAppAction(nameof(ArticlesController.GetArticles), "about_articles",
				new Dictionary<string, string> {
					{ "size", "10" },
					{ "offset", "0" },
					{ "pageId", "1" } }));

			return Ok(responseModel);
		}
	}
}
