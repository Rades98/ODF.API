using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODF.Data.Elastic.Repos
{
	internal static class ElasticScriptExtensions
	{
		internal static void AddIfEdited(this object value, Dictionary<string, object> scriptParams, StringBuilder script)
		{
			if (value is string stringVal)
			{
				if (!string.IsNullOrEmpty(stringVal))
				{
					scriptParams.Add(nameof(stringVal), stringVal);
					script.Append($"item.email = params.{nameof(stringVal)};");
				}
			}
			else if (value is int?)
			{
				var intVal = (int?)value;
				if (intVal is not null)
				{
					scriptParams.Add(nameof(intVal), intVal);
					script.Append($"item.order = params.{nameof(intVal)};");
				}
			}
			else if (value is IEnumerable<string> enumerable)
			{
				if (enumerable.Any())
				{
					scriptParams.Add(nameof(enumerable), enumerable);
					script.Append($"item.roles = params.{nameof(enumerable)};");
				}
			}
			else if (value is DateTime dateTime)
			{
				scriptParams.Add(nameof(dateTime), dateTime.ToString("yyyy-MM-ddTHH:mm:ss"));
				script.Append($"ctx._source.dateTime = params.{nameof(dateTime)};");
			}
			else
			{
				throw new ArgumentException("No conversion provided", nameof(value));
			}
		}
	}
}
