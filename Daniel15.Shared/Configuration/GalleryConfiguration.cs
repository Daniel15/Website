using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Daniel15.Shared.Configuration;

namespace Daniel15.Configuration
{
	public class GalleryConfiguration : ConfigurationSection, IGalleryConfiguration
	{
		/// <summary>
		/// Gets all the configured image galleries
		/// </summary>
		[ConfigurationProperty("galleries")]
		public GalleryCollection Galleries
		{
			get { return (GalleryCollection)this["galleries"]; }
			set { this["galleries"] = value; }
		}

		IDictionary<string, IGallery> IGalleryConfiguration.Galleries
		{
			get
			{
				// Galleries is an IDictionary<string, Gallery>.
				// This cannot be directly casted to IDictionary<string, IGallery> as dictionaries are not covariant :(
				return ((IDictionary<string, Gallery>) Galleries).ToDictionary(x => x.Key, x => (IGallery)x.Value);
			}
		}
	}

	public class Gallery : ConfigurationSection, IGallery
	{
		/// <summary>
		/// Gets the name of the image gallery
		/// </summary>
		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get { return (string) this["name"]; }
			set { this["name"] = value; }
		}

		/// <summary>
		/// Gets the URL images in this gallery are directly accessible at
		/// </summary>
		[ConfigurationProperty("imageUrl", IsRequired = true)]
		public Uri ImageUrl
		{
			get { return (Uri)this["imageUrl"]; }
			set { this["imageUrl"] = value; }
		}

		/// <summary>
		/// Gets the directory images for this gallery are stored in
		/// </summary>
		[ConfigurationProperty("imageDir", IsRequired = true)]
		public string ImageDir
		{
			get { return (string)this["imageDir"]; }
			set { this["imageDir"] = value; }
		}
	}

	[ConfigurationCollection(typeof(GalleryCollection))]
	public class GalleryCollection : ConfigurationElementCollection<string, Gallery>
	{
		public override string GetElementKey(Gallery element)
		{
			return element.Name;
		}
	}
}
