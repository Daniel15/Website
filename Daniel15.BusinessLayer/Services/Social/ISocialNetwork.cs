namespace Daniel15.BusinessLayer.Services.Social
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
