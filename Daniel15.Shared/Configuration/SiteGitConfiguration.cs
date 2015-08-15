namespace Daniel15.Shared.Configuration
{
	/// <summary>
	/// Information about the Git commit the site is running on
	/// </summary>
    public class SiteGitConfiguration
    {
		/// <summary>
		/// Gets the date of the commit the site is running on, as a UNIX timestamp
		/// </summary>
		public long Date { get; set; }

		/// <summary>
		/// Gets the URL to the Git revision the site is running on
		/// </summary>
		public string RevisionUrl { get; set; }

		/// <summary>
		/// Gets the Git revision the site is running on
		/// </summary>
		public string Revision { get; set; }
    }
}
