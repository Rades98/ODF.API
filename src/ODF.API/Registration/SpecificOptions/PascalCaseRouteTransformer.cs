namespace ODF.API.Registration.SpecificOptions
{
	public class PascalCaseRouteTransformer : IOutboundParameterTransformer
	{
		public string? TransformOutbound(object? value)
		{
			if (value == null)
			{
				return null;
			}

			string input = value.ToString()!;

			return string.Concat(input[0].ToString().ToLower(), input.AsSpan(1));
		}
	}
}
