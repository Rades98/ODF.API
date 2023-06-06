using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.Registration.SettingModels;
using ODF.API.ResponseModels.Association;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;

namespace ODF.API.Controllers
{
	public class AssociationController : BaseController
	{
		public AssociationController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetAssociation))]
		[ProducesResponseType(typeof(AssociationResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAssociation([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);
			var responseModel = new AssociationResponseModel(translations.Get("association_info"), translations.Get("association_header"));

			responseModel.AddAction(GetQueriedAppAction(nameof(ArticlesController.GetArticles), "about_articles",
				new Dictionary<string, string> {
					{ "size", "10" },
					{ "offset", "0" },
					{ "pageId", "1" } }));

			return Ok(responseModel);
		}
	}
}
