namespace Daniel15.Web.Services.Social
{
	/// <summary>
	/// Represents general details of a social network
	/// </summary>
	public interface ISocialNetwork
	{
		/// <summary>
		/// Gets the internal ID of this social network
		/// </summary>
		string Id { get; }

		/// <summary>
		/// Gets the friendly name of this social network
		/// </summary>
		string Name { get; }
	}
}
