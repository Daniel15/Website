using System.Collections.Generic;

namespace Daniel15.Web.Configuration
{
	public interface IGalleryConfiguration
	{
		/// <summary>
		/// Gets all the configured image galleries
		/// </summary>
		Dictionary<string, Gallery> Galleries { get; }
	}
}
