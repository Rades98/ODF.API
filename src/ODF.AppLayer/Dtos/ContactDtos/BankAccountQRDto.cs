using System.Text;

namespace ODF.AppLayer.Dtos.ContactDtos
{
	/// <summary>
	/// https://qr-platba.cz/pro-vyvojare/specifikace-formatu/
	/// </summary>
	public class BankAccountQRDto
	{
		public string IBAN { get; set; } //IBAN

		public int? InternalPaymentId { get; set; } //internal payment identifier

		public string VariableSymbol { get; set; } = string.Empty; // X-VS variable symbol

		public string SpecificSymbol { get; set; } = string.Empty; // X-SS specific symbol

		public string ConstantSymbol { get; set; } = string.Empty; // X-KS constant symbol

		public string AccountId { get; set; } = string.Empty;

		public override string ToString()
		{
			string ibanFormated = IBAN.Replace(" ", "").ToUpper();
			var builder = new StringBuilder($"SPD*1.0*ACC:{ibanFormated}*PT:IP*CC:CZK*AM:1.0*RN:FolklorOVA z.s.");

			if (InternalPaymentId is not null)
			{
				builder.Append($"*RF{InternalPaymentId}");
			}

			if (!string.IsNullOrEmpty(VariableSymbol))
			{
				builder.Append($"*X-VS{VariableSymbol}");
			}

			if (!string.IsNullOrEmpty(SpecificSymbol))
			{
				builder.Append($"*X-SS{SpecificSymbol}");
			}

			if (!string.IsNullOrEmpty(ConstantSymbol))
			{
				builder.Append($"*X-KS{ConstantSymbol}");
			}

			return builder.ToString();
		}
	}
}
