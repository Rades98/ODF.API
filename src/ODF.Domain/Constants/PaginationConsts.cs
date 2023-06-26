using System.Collections.Generic;

namespace ODF.Domain.Constants
{
	public static class PaginationConsts
	{
		private static Dictionary<string, string> _defaultPaginationSetting(int pageId) => new()
			{
				{ "size", "10" },
				{ "offset", "0" },
				{ "pageId", $"{pageId}" }
			};

		public static Dictionary<string, string> DefaultPaginationSetting(int pageId)
			=> _defaultPaginationSetting(pageId);
	}
}
