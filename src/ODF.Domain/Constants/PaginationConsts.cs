using System.Collections.Generic;

namespace ODF.Domain.Constants
{
	public static class PaginationConsts
	{
		private static readonly Dictionary<string, string> _defaultPaginationSetting = new()
			{
				{ "size", "10" },
				{ "offset", "0" },
				{ "pageId", "1" }
			};

		public static Dictionary<string, string> DefaultPaginationSetting
			=> _defaultPaginationSetting;
	}
}
