using System;

namespace ODF.AppLayer.Exceptions
{
	public class MissingTranslationException : Exception
	{
		public MissingTranslationException(string message) : base(message)
		{

		}
	}
}
