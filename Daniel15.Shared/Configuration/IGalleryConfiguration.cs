using System.Collections.Generic;

namespace Daniel15.Shared.Configuration
{
	public interface IGalleryConfiguration
	{
		/// <summary>
		/// Gets all the configured image galleries
		/// </summary>
		Dictionary<string, Gallery> Galleries { get; }
	}
}
