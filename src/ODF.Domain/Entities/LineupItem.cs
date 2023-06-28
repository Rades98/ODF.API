using System;

namespace ODF.Domain.Entities
{
	public class LineupItem
	{
		public Guid Id { get; set; }

		public DateTime DateTime { get; set; }

		public string Place { get; set; } = string.Empty;

		public string Interpret { get; set; } = string.Empty;

		public string PerformanceName { get; set; } = string.Empty;

		public string DescriptionTranslation { get; set; } = string.Empty;

		public string? UserName { get; set; }

		public string? UserNote { get; set; }
	}
}
