using System;
namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Helpful date methods
	/// </summary>
	public static class DateExtensions
	{
		/// <summary>
		/// Epoch used for Unix timestamps
		/// </summary>
		private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1);

		/// <summary>
		/// Converts this <see cref="DateTime"/> into a UNIX timestamp.
		/// </summary>
		/// <param name="dateTime">The date to convert</param>
		/// <returns>UNIX timestamp</returns>
		public static long ToUnix(this DateTime dateTime)
		{
			return Convert.ToInt64((dateTime - UNIX_EPOCH.ToLocalTime()).TotalSeconds);
		}

		/// <summary>
		/// Converts this UNIX timestamp into a <see cref="DateTime"/>.
		/// </summary>
		/// <param name="unixTimeStamp">The unix time stamp.</param>
		/// <returns>Date represented by this UNIX timestamp</returns>
		public static DateTime FromUnix(long unixTimeStamp)
		{
			return UNIX_EPOCH.AddSeconds(unixTimeStamp).ToLocalTime();
		}
	}
}