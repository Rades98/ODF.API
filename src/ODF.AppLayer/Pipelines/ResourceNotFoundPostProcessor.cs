using System;
using System.Runtime.Serialization;
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

	[Serializable]
	public class TranslationNotFoundException : Exception
	{
		public TranslationNotFoundException()
		{
		}

		public TranslationNotFoundException(string message)
			: base(message)
		{
		}

		public TranslationNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected TranslationNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
