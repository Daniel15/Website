using System.Collections.Generic;

namespace Daniel15.Configuration
{
	public interface IGalleryConfiguration
	{
		/// <summary>
		/// Gets all the configured image galleries
		/// </summary>
		IDictionary<string, IGallery> Galleries { get; }
	}
}
