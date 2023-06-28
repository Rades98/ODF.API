using System;
using ODF.AppLayer.CQRS.Interfaces.Lineup;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Lineup.Commands
{
	public sealed class AddLineupItemCommand : ICommand<ValidationDto>, IAddLineupItem
	{
		public AddLineupItemCommand(string place, string interpret, string perfName, string description,
			string descriptionTranslationCode, DateTime dateTime, string countryCode, string userName = null, string userNote = null)
		{
			Place = place;
			Interpret = interpret;
			PerformanceName = perfName;
			DescriptionTranslationCode = descriptionTranslationCode;
			Description = description;
			DateTime = dateTime;
			CountryCode = countryCode;
			UserName = userName;
			UserNote = userNote;
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
