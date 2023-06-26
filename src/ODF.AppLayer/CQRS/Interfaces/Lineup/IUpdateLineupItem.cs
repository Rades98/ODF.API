using System;

namespace ODF.AppLayer.CQRS.Interfaces.Lineup
{
	public interface IUpdateLineupItem
	{
		Guid Id { get; }

		string Place { get; }

		string Interpret { get; }

		string PerformanceName { get; }

		string Description { get; }

		string DescriptionTranslationCode { get; }

		DateTime? DateTime { get; }

		string UserName { get; }
	}
}
