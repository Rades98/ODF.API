using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ODF.AppLayer.Consts;
using ODF.AppLayer.CQRS.User.Commands;
using ODF.AppLayer.Dtos;

namespace ODF.AppLayer.CQRS.User.CommandHandlers
{
	internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserDto>
	{
		public Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			return Task.FromResult(new UserDto()
			{
				UserName = "Admin",
				Email = "admin@folklorova.cz",
				Claims = new List<Claim>()
				{
					new Claim("Id", Guid.NewGuid().ToString()),
					new Claim(ClaimTypes.Role, UserRoles.Admin),
				}
			});
		}
	}
}
