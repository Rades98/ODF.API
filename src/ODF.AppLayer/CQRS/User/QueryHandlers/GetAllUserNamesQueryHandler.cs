using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ODF.AppLayer.CQRS.User.Queries;
using ODF.AppLayer.Mediator;
using ODF.AppLayer.Repos;

namespace ODF.AppLayer.CQRS.User.QueryHandlers
{
	internal class GetAllUserNamesQueryHandler : IQueryHandler<GetAllUserNamesQuery, IEnumerable<string>>
	{
		private readonly IUserRepo _userRepo;

		public GetAllUserNamesQueryHandler(IUserRepo userRepo)
		{
			_userRepo = userRepo ?? throw new ArgumentNullException(nameof(userRepo));
		}

		public Task<IEnumerable<string>> Handle(GetAllUserNamesQuery request, CancellationToken cancellationToken)
			=> (_userRepo.GetUserNamesAsync(cancellationToken));
	}
}
