namespace Daniel15.Web.Configuration
{
	/// <summary>
	/// An implementation of <see cref="ISiteConfiguration"/>.
	/// section.
	/// </summary>
	public class SiteConfiguration : ISiteConfiguration
	{
		/// <summary>
		/// Gets the name of the blog
		/// </summary>
		public string BlogName { get; set; }

		/// <summary>
		/// Gets the description of the blog
		/// </summary>
		public string BlogDescription { get; set; }

		/// <summary>
		/// Gets the Disqus shortname used by the blog comments
		/// </summary>
		public string DisqusShortname { get; set; }

		/// <summary>
		/// Gets the Disqus API key (used for syncing comments)
		/// </summary>
		public string DisqusApiKey { get; set; }

		/// <summary>
		/// Gets the Disqus category ID (used for syncing comments)
		/// </summary>
		public int DisqusCategory { get; set; }

		/// <summary>
		/// Gets information about the Git commit the site is running on
		/// </summary>
		public SiteGitConfiguration Git { get; set; }
	}
}
