using System;

namespace ODF.AppLayer.Dtos
{
	public class LineupItemDto
	{
		public Guid Id { get; set; }

		public DateTime DateTime { get; set; }

		public string Place { get; set; } = string.Empty;

		public string Interpret { get; set; } = string.Empty;

		public string PerformanceName { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public string DescriptionTranslationCode { get; set; } = string.Empty;

		public string UserName { get; set; } = string.Empty;

		public string UserNote { get; set; } = string.Empty;
	}
}
