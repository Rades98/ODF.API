using System.Collections.Generic;

namespace ODF.Domain.Constants
{
	public static class PaginationConsts
	{
		public static readonly Dictionary<string, string> DefaultPaginationSetting = new()
			{
				{ "size", "10" },
				{ "offset", "0" },
				{ "pageId", "1" }
			};
	}
}
