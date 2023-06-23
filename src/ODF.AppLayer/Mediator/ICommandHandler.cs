using MediatR;
using ODF.AppLayer.Dtos.Interfaces;

namespace ODF.AppLayer.Mediator
{
	public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : class, ICommand<TResponse> where TResponse : IValidationDto
	{
	}
}
