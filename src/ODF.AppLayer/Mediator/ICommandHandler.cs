using MediatR;

namespace ODF.AppLayer.Mediator
{
	public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> where TCommand : class, ICommand<TResponse>
	{
	}
}
