using System;
using ODF.AppLayer.CQRS.Interfaces.Lineup;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Lineup.Commands
{
	public sealed class DeleteLineupItemCommand : ICommand<ValidationDto>, IDeleteLineupItem
	{
		public DeleteLineupItemCommand(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}
