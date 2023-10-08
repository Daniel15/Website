using System.Collections.Generic;

namespace Daniel15.Web.Configuration
{
	public class GalleryConfiguration : IGalleryConfiguration
	{
		/// <summary>
		/// Gets all the configured image galleries
		/// </summary>
		public Dictionary<string, Gallery> Galleries { get; set; }
	}
}
