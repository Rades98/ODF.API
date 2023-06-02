using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Dtos.User;
using ODF.Data.Contracts.Interfaces;

namespace ODF.AppLayer.CQRS.User.CommandHandlers
{
	internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserValidationDto>
	{
		private readonly IUserRepo _userRepo;

		public LoginUserCommandHandler(IUserRepo userRepo)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
		}

		public async Task<UserValidationDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepo.GetUserAsync(request.UserName, cancellationToken);

			if (user is not null)
			{
				var claims = new List<Claim>()
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(ClaimTypes.Actor, user.Id.ToString())
				};

				var result = new UserValidationDto()
				{
					IsOk = true,
					User = new UserDto()
					{
						UserName = user.UserName,
						FirstName = user.FirstName,
						LastName = user.LastName,
						Email = user.Email,
					}
				};

				if (user.IsAdmin)
				{
					claims.Add(new Claim(ClaimTypes.Role, UserRoles.Admin));
				}

				result.User.Claims = claims;

				return result;
			}

			return new() { IsOk = false };
		}
	}
}
