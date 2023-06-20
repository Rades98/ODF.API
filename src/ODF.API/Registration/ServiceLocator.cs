namespace ODF.API.Registration
{
	[Obsolete("This is freaking hack.. Find out better way to get httpcontext current in Attributes")]
	public static class ServiceLocator
	{
		public static IServiceProvider? Instance { get; set; }
	}
}
