using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.Dtos.Interfaces;

namespace ODF.AppLayer.Pipelines
{
	public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TResponse : class, IValidationDto where TRequest : IRequest<TResponse>
	{
		private readonly IEnumerable<IValidator<TRequest>> _validators;
		private readonly ILogger<TRequest> _logger;

		public ValidationPipeline(IEnumerable<IValidator<TRequest>> validators, ILogger<TRequest> logger) => (_validators, _logger) = (validators, logger);

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			var context = new ValidationContext<TRequest>(request);

			var failures = _validators
				.Select(validation => validation.ValidateAsync(context))
				.SelectMany(result => result.Result.Errors)
				.Where(failure => failure != null && failure.Severity == Severity.Error)
				.ToList();

			if (failures.Count != 0)
			{
				failures.ForEach(f => _logger.LogError("{errorCode} : {errorMessage}", f.ErrorMessage, f.ErrorCode));
				var response = (IValidationDto)Activator.CreateInstance(typeof(TResponse));
				response.IsOk = false;
				response.Errors = failures;
				return (TResponse)response;
			}

			return await next();
		}
	}
}
