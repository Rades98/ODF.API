using ODF.API.RequestModels.Forms.Lineup;
using ODF.AppLayer.Dtos;

namespace ODF.API.Extensions.MappingExtensions
{
	public static class LineupDtoToFormMapper
	{
		public static UpdateLineupItemForm ToUpdateForm(this LineupItemDto model)
			=> new()
			{
				DateTime = model.DateTime,
				Description = model.Description,
				DescriptionTranslationCode = model.DescriptionTranslationCode,
				Id = model.Id,
				Interpret = model.Interpret,
				PerformanceName = model.PerformanceName,
				Place = model.Place,
				UserName = model.UserName,
				UserNote = model.UserNote,
			};
	}
}
