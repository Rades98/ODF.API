using System.Collections.Generic;

namespace ODF.API.Registration.SettingModels
{
	/// <summary>
	/// Elastic search settings
	/// </summary>
	internal class ElasticsearchSettings
	{
		/// <summary>
		/// Nodes
		/// </summary>
		public ICollection<string> Nodes { get; set; } = new List<string>();

		/// <summary>
		/// Default index
		/// </summary>
		public string DefaultIndex { get; set; } = string.Empty;

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; set; } = string.Empty;
	}
}
