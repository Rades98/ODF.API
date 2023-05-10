using System;
using MediatR;

namespace ODF.AppLayer.CQRS.Lineup.Commands
{
	public class AddLineupItemCommand : IRequest<bool>
	{
		public AddLineupItemCommand(string place, string interpret, string perfName, string description, string descriptionTranslationCode, DateTime dateTime, string countryCode)
		{
			Place = place;
			Interpret = interpret;
			PerformanceName = perfName;
			DescriptionTranslationCode = descriptionTranslationCode;
			Description = description;
			DateTime = dateTime;
			CountryCode = countryCode;
		}

		public string Place { get; }

		public string Interpret { get; }

		public string PerformanceName { get; }

		public string Description { get; }

		public string DescriptionTranslationCode { get; }

		public DateTime DateTime { get; }

		public string CountryCode { get; }
	}
}
