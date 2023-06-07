using MediatR;

namespace ODF.AppLayer.Mediator
{
	public interface IQuery<out TResponse> : IRequest<TResponse>
	{
	}
}
