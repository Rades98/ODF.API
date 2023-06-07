using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ODF.AppLayer.Dtos.Interfaces;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.Pipelines
{
	public class ValidationPipeline<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TResponse : class, IValidationDto where TCommand : ICommand<TResponse>
	{
		private readonly IEnumerable<IValidator<TCommand>> _validators;
		private readonly ILogger<TCommand> _logger;

		public ValidationPipeline(IEnumerable<IValidator<TCommand>> validators, ILogger<TCommand> logger) => (_validators, _logger) = (validators, logger);

		public async Task<TResponse> Handle(TCommand request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
		{
			var context = new ValidationContext<TCommand>(request);

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
