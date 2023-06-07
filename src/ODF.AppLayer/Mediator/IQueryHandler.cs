using MediatR;

namespace ODF.AppLayer.Mediator
{
	public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : class, IQuery<TResponse>
	{
	}
}
