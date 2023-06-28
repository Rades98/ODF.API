using ODF.AppLayer.Dtos;
using ODF.Domain.Entities;

namespace ODF.AppLayer.CQRS.Lineup
{
	internal static class MappingExtensions
	{
		internal static LineupItemDto MapLineupItem(this LineupItem lineupItem, string description)
		{
			if (!string.IsNullOrEmpty(description))
			{
				return new()
				{
					Id = lineupItem.Id,
					DateTime = lineupItem.DateTime,
					Description = description,
					Interpret = lineupItem.Interpret,
					PerformanceName = lineupItem.PerformanceName,
					Place = lineupItem.Place,
					DescriptionTranslationCode = lineupItem.DescriptionTranslation,
					UserName = lineupItem.UserName,
					UserNote = lineupItem.UserNote,
				};
			}

			return null;
		}
	}
}
