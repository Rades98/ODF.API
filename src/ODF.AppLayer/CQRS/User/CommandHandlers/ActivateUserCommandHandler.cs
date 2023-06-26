using System;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.User.CommandHandlers
{
	internal class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand, ValidationDto>
	{
		private readonly IUserRepo _userRepo;

		public ActivateUserCommandHandler(IUserRepo userRepo)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
		}

		public async Task<ValidationDto> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepo.GetUserByHashAsync(request.Hash, cancellationToken);

			if (user is null)
			{
				return ValidationDto.Invalid;
			}

			return new() { IsOk = await _userRepo.ActivateRegistration(user.UserName, cancellationToken) };
		}
	}
}
