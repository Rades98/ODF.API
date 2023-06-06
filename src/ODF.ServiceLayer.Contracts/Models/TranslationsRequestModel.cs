using System.Collections.Generic;


namespace ODF.ServiceLayer.Contracts.Models
{
	public class TranslationsRequestModel 
	{
		public TranslationsRequestModel(string countryCode, int size, int offset)
		{
			CountryCode = countryCode;
			Size = size;
			Offset = offset;
		}

		public string CountryCode { get; }

		public int Size { get; }

		public int Offset { get; }
	}
}
