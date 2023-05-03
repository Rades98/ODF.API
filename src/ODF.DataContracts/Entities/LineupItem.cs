using System;

namespace ODF.Data.Contracts.Entities
{
	public class LineupItem
	{
		public DateTime DateTime { get; set; }

		public string Place { get; set; } = string.Empty;

		public string Interpret { get; set; } = string.Empty;

		public string PerformanceNameTranslation { get; set; } = string.Empty;

		public string DescriptionTranslation { get; set; } = string.Empty;
	}
}
