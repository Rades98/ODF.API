using System;
using ODF.AppLayer.CQRS.Interfaces.Lineup;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Lineup.Commands
{
	public sealed class AddLineupItemCommand : ICommand<ValidationDto>, IAddLineupItem
	{
		public AddLineupItemCommand(IAddLineupItem input, string countryCode)
		{
			Place = input.Place;
			Interpret = input.Interpret;
			PerformanceName = input.PerformanceName;
			DescriptionTranslationCode = input.DescriptionTranslationCode;
			Description = input.Description;
			DateTime = input.DateTime;
			CountryCode = countryCode;
			UserName = input.UserName;
			UserNote = input.UserNote;
		}

		public string Place { get; }

		public string Interpret { get; }

		public string PerformanceName { get; }

		public string Description { get; }

		public string DescriptionTranslationCode { get; }

		public DateTime DateTime { get; }

		public string CountryCode { get; }

		public string UserName { get; } = null;

		public string UserNote { get; }
	}
}
