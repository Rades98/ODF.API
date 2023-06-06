using System;

namespace ODF.AppLayer.Exceptions
{
	public class UnsupportedLanguageException : Exception
	{
		public UnsupportedLanguageException(string message) : base(message)
		{

		}
	}
}
