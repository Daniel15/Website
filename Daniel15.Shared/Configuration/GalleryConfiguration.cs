using System.Collections.Generic;

namespace Daniel15.Shared.Configuration
{
	public class GalleryConfiguration : IGalleryConfiguration
	{
		/// <summary>
		/// Gets all the configured image galleries
		/// </summary>
		public Dictionary<string, Gallery> Galleries { get; set; }
	}
}
