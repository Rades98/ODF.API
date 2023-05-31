using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ODF.API.FormFactories;
using ODF.API.Registration.SettingModels;
using ODF.API.RequestModels.Forms;
using ODF.API.ResponseModels.About;
using ODF.API.ResponseModels.Common;
using ODF.API.ResponseModels.Exceptions;
using ODF.API.ResponseModels.User;
using ODF.API.Responses;
using ODF.AppLayer.CQRS.Translations.Queries;
using ODF.AppLayer.CQRS.User.Commands;
using System.Security.Claims;
using ODF.API.Extensions;

namespace ODF.API.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApiSettings _settings;

        public UsersController(IMediator mediator, IOptions<ApiSettings> apiSettings)
        {
            _mediator = mediator;
            _settings = apiSettings.Value;
        }


        [HttpPost("/{countryCode}/user/login")]
        [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UnauthorizedExceptionResponseModel), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserRequestForm user, [FromRoute] string countryCode, CancellationToken cancellationToken)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var loginTranslation = await _mediator.Send(new GetTranslationQuery("Uživatelské jméno", "login_username", countryCode), cancellationToken);
            var passwordTranslation = await _mediator.Send(new GetTranslationQuery("Heslo", "login_pw", countryCode), cancellationToken);

            // MOCK
            if (user.UserName == "admin" && user.Password == "heslopyco")
            {
                var userResult = await _mediator.Send(new LoginUserCommand("admin", "adminPW"), cancellationToken); //work with mock

                var claimsIdentity = new ClaimsIdentity(userResult.Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                var responseModel = new UserResponseModel(_settings.ApiUrl, userResult.UserName, countryCode, UserFormFactory.GetLoginForm(loginTranslation, passwordTranslation));
                responseModel.AddAction($"/{countryCode}/navigation", "nav", HttpMethods.Get);

                return Ok(responseModel);
            }

            var password2Translation = await _mediator.Send(new GetTranslationQuery("Heslo pro kontrolu", "login_pw2", countryCode), cancellationToken);
            var emailTranslation = await _mediator.Send(new GetTranslationQuery("e-mail", "login_email", countryCode), cancellationToken);
            var firstNameTranslation = await _mediator.Send(new GetTranslationQuery("Jméno", "login_first_name", countryCode), cancellationToken);
            var lastNameTranslation = await _mediator.Send(new GetTranslationQuery("Příjmení", "login_last_name", countryCode), cancellationToken);

            var title = await _mediator.Send(new GetTranslationQuery("Přihlášení se nezdařilo", "login_failed_title", countryCode), cancellationToken);
            string message = await _mediator.Send(new GetTranslationQuery("Zkontrolujte, že jste zadali správné údaje k účtu", "login_failed_msg", countryCode), cancellationToken);

            string registrationActionName = await _mediator.Send(new GetTranslationQuery("Nemáte registraci? Klikněte zde!", "register_action_name", countryCode), cancellationToken);

            var registerAction = new NamedAction(_settings.ApiUrl + $"/{countryCode}/user", registrationActionName, "register", HttpMethods.Put,
                UserFormFactory.GetRegisterForm(loginTranslation, passwordTranslation, password2Translation, emailTranslation, firstNameTranslation, lastNameTranslation));

            return (IActionResult)CustomApiResponses.Unauthorized(new UnauthorizedExceptionResponseModel(title, message, registerAction));
        }

        [HttpPut("/{countryCode}/user/register")]
        public IActionResult RegisterUser([FromRoute] string countryCode)
        {
            return Ok(); 
        }

        [HttpPost("/{countryCode}/user/logout")]
        public async Task<IActionResult> LogoutUser([FromRoute] string countryCode, CancellationToken cancellationToken)
        {
            if (HttpContext.IsLoggedIn())
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Accepted();
            }

            return UnprocessableEntity();
        }
    }
}
