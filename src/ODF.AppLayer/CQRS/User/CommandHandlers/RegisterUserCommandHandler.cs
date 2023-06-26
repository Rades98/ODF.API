using System;
using System.Threading;
using System.Threading.Tasks;
using MailingService.Client.Interfaces;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Extensions;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;
using ODF.AppLayer.Services.Interfaces;
using ODF.Domain.Utils;

namespace ODF.AppLayer.CQRS.User.CommandHandlers
{
	internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, ValidationDto>
	{
		private readonly IUserRepo _userRepo;
		private readonly IMailSender _mailSender;
		private readonly ITranslationsProvider _translationsProvider;

		public RegisterUserCommandHandler(IUserRepo userRepo, IMailSender mailSender, ITranslationsProvider translationsProvider)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
			_mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
			_translationsProvider = translationsProvider ?? throw new ArgumentNullException(nameof(translationsProvider));
		}

		public async Task<ValidationDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			string hash = await _userRepo.RegisterAsync(request.UserName, request.Email, false, Hasher.Hash(request.Password), cancellationToken);

			if (!string.IsNullOrEmpty(hash))
			{
				var translations = await _translationsProvider.GetTranslationsAsync(request.CountryCode, cancellationToken);
				await _mailSender.SendRegistrationEmailAsync(new()
				{
					ActivationUrl = new(request.ActiavtionLinkTemplate.Replace("{hash}", hash)),
					ConfirmButtonText = translations.Get("registration_html_confirm"),
					DeleteNoticeIfSpam = translations.Get("registration_html_spamnotice"),
					Email = request.Email,
					Header = string.Format(translations.Get("registration_html_header"), $"{request.FirstName} {request.LastName}"),
					Info = translations.Get("registration_html_info"),
					Name = request.FirstName,
					NotWorkMessage = translations.Get("registration_html_infoerr"),
					Regards = translations.Get("registration_html_regards"),
					Subject = translations.Get("registration_email_header")
				}, cancellationToken);
				return new() { IsOk = true };
			}

			return ValidationDto.Invalid;
		}
	}
}
