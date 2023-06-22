using System;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Lineup.Commands
{
	public sealed class DeleteLineupItemCommand : ICommand<ValidationDto>
	{
		public DeleteLineupItemCommand(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}
