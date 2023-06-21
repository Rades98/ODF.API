using System;
using System.Runtime.Serialization;

namespace ODF.AppLayer.Exceptions
{
	[Serializable]
	public class UnsupportedLanguageException : Exception
	{
		public UnsupportedLanguageException()
		{
		}

		public UnsupportedLanguageException(string message)
			: base(message)
		{
		}

		public UnsupportedLanguageException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected UnsupportedLanguageException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
