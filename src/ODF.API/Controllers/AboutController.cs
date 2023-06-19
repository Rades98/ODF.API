using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using ODF.API.Controllers.Base;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Exceptions;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.SettingModels;

namespace ODF.API.Controllers
{
	public class AboutController : BaseController
	{
		public AboutController(IMediator mediator, IOptions<ApiSettings> apiSettings, IActionDescriptorCollectionProvider adcp, ITranslationsProvider translationsProvider) : base(mediator, apiSettings, adcp, translationsProvider)
		{
		}

		[HttpGet(Name = nameof(GetAbout))]
		[ProducesResponseType(typeof(AboutResponseModel), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ExceptionResponseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetAbout([FromRoute] string countryCode, CancellationToken cancellationToken)
		{
			var translations = await TranslationsProvider.GetTranslationsAsync(countryCode, cancellationToken);

			var responseModel = new AboutResponseModel(translations.Get("about_info"), translations.Get("about_header"));

			responseModel.AddAction(GetQueriedAppAction(nameof(ArticlesController.GetArticles), "about_articles",
				new Dictionary<string, string> {
					{ "size", "10" },
					{ "offset", "0" },
					{ "pageId", "0" } }));

			return Ok(responseModel);
		}
	}
}
