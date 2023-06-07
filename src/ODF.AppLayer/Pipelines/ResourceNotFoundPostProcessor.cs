using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.Pipelines
{
	internal class ResourceNotFoundPostProcessor<TQuery, TResponse> : IRequestPostProcessor<TQuery, TResponse> where TQuery : notnull, IQuery<TResponse>
	{
		public Task Process(TQuery request, TResponse response, CancellationToken cancellationToken)
		{
			if (response is null)
			{
				throw new TranslationNotFoundException("some additional info in the future");
			}

			return Task.CompletedTask;
		}
	}

	public class TranslationNotFoundException : Exception
	{
		public TranslationNotFoundException(string message) : base(message)
		{

		}
	}
}
