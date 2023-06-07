using MediatR;

namespace ODF.AppLayer.Mediator
{
	public interface ICommand<out TResponse> : IRequest<TResponse>
	{
	}
}
