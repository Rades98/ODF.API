using System;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Lineup.Commands
{
	public class DeleteLineupItemCommand : ICommand<ValidationDto>
	{
		public DeleteLineupItemCommand(Guid id)
		{
			Id = id;
		}

		public Guid Id { get; }
	}
}
