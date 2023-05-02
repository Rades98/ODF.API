using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace ODF.AppLayer.Pipelines
{
	internal class ResourceNotFoundPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
	{
		public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
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
