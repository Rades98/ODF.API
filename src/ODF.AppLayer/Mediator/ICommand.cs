using MediatR;
using ODF.AppLayer.Dtos.Interfaces;

namespace ODF.AppLayer.Mediator
{
	public interface ICommand<out TResponse> : IRequest<TResponse> where TResponse : IValidationDto
	{
	}
}
