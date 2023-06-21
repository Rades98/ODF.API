using System;
using System.Runtime.Serialization;

namespace ODF.AppLayer.Exceptions
{
	[Serializable]
	public class MissingTranslationException : Exception
	{
		public MissingTranslationException()
		{
		}

		public MissingTranslationException(string message)
			: base(message)
		{
		}

		public MissingTranslationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected MissingTranslationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
