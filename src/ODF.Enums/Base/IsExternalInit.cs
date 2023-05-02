using System.ComponentModel;

namespace ODF.Enums.Base
{
	/// <summary>
	/// Reserved to be used by the compiler for tracking metadata.
	/// This class should not be used by developers in source code.
	/// This dummy class is required to compile records when targeting .NET Standard
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class IsExternalInit
	{
	}
}