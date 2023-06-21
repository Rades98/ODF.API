using System;
using System.Runtime.Serialization;

namespace ODF.AppLayer.Exceptions
{
	public class MissingTranslationException : Exception, ISerializable
	{
		public MissingTranslationException(string message) : base(message)
		{

		}
	}
}
