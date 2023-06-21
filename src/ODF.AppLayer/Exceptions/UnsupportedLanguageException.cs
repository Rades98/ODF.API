using System;
using System.Runtime.Serialization;

namespace ODF.AppLayer.Exceptions
{
	public class UnsupportedLanguageException : Exception, ISerializable
	{
		public UnsupportedLanguageException(string message) : base(message)
		{

		}
	}
}
