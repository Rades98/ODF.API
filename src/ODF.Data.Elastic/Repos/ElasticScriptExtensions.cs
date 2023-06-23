using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ODF.Domain.Extensions;

namespace ODF.Data.Elastic.Repos
{
	internal static class ElasticScriptExtensions
	{
		internal static void AddToScriptWithParamIsfEdited(this object value, string propName, Dictionary<string, object> scriptParams, StringBuilder script, string prefix = null)
		{
			string sourceKind = prefix ?? "ctx._source";
			string normalizedPropName = propName.ToCamelCase();

			if (value is string stringVal)
			{
				if (!string.IsNullOrEmpty(stringVal))
				{
					scriptParams.Add(normalizedPropName, stringVal);
					script.Append($"{sourceKind}.{normalizedPropName} = params.{normalizedPropName};");
				}
			}
			else if (value is int?)
			{
				int? intVal = (int?)value;
				if (intVal is not null)
				{
					scriptParams.Add(normalizedPropName, intVal);
					script.Append($"{sourceKind}.{normalizedPropName} = params.{normalizedPropName};");
				}
			}
			else if (value is IEnumerable<string> enumerable)
			{
				if (enumerable.Any())
				{
					scriptParams.Add(normalizedPropName, enumerable);
					script.Append($"{sourceKind}.{normalizedPropName} = params.{normalizedPropName};");
				}
			}
			else if (value is DateTime dateTime)
			{
				scriptParams.Add(normalizedPropName, dateTime.ToString("yyyy-MM-ddTHH:mm:ss"));
				script.Append($"{sourceKind}.{normalizedPropName} = params.{normalizedPropName};");
			}
			else
			{
				throw new ArgumentException("No conversion provided", normalizedPropName);
			}
		}
	}
}
