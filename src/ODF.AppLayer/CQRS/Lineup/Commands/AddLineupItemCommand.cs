using System;
using ODF.AppLayer.Dtos.Validation;
using ODF.AppLayer.Mediator;

namespace ODF.AppLayer.CQRS.Lineup.Commands
{
	public class AddLineupItemCommand : ICommand<ValidationDto>
	{
		public AddLineupItemCommand(string place, string interpret, string perfName, string description,
			string descriptionTranslationCode, DateTime dateTime, string countryCode, string userName = null)
		{
			Place = place;
			Interpret = interpret;
			PerformanceName = perfName;
			DescriptionTranslationCode = descriptionTranslationCode;
			Description = description;
			DateTime = dateTime;
			CountryCode = countryCode;
			UserName = userName;
		}

		public string Place { get; }

		public string Interpret { get; }

		public string PerformanceName { get; }

		public string Description { get; }

		public string DescriptionTranslationCode { get; }

		public DateTime DateTime { get; }

		public string CountryCode { get; }

		public string UserName { get; } = null;
	}
}
