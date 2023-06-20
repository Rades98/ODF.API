using ODF.Domain.Extensions;

namespace ODF.API.Registration.SpecificOptions
{
	public class CamelCaseRouteTransformer : IOutboundParameterTransformer
	{
		public string? TransformOutbound(object? value)
		{
			if (value == null)
			{
				return null;
			}

			string input = value.ToString()!;

			return input.ToCamelCase();
		}
	}
}
