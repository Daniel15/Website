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
		private static DateTime _unixEpoch = new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc);

		/// <summary>
		/// Number of dates in a year
		/// </summary>
		private const double DAYS_IN_YEAR = 365.24;

		/// <summary>
		/// Like ToString but inserts the ordinal as well (1st, 2nd, etc.). Put {0} where you want
		/// the ordinal to appear
		/// </summary>
		/// <param name="dateTime">Date to format</param>
		/// <param name="format">Format string to use</param>
		/// <returns>Date formatted and with ordinal inserted</returns>
		public static string ToStringWithOrdinal(this DateTime dateTime, string format = "d{0} MMMM yyyy")
		{
			return string.Format(dateTime.ToString(format), Ordinal(dateTime.Day));
		}
		
		/// <summary>
		/// Gets the ordinal for the specified number (ie. "st" for 1, "nd" for 2, etc.)
		/// </summary>
		/// <param name="num">Number</param>
		/// <returns>The ordinal</returns>
		public static string Ordinal(int num)
		{
			switch (num % 100)
			{
				case 11:
				case 12:
				case 13:
					return "th";
			}

			switch (num % 10)
			{
				case 1:
					return "st";
				case 2:
					return "nd";
				case 3:
					return "rd";
				default:
					return "th";
			}
		}

		/// <summary>
		/// Converts this <see cref="DateTime"/> into a UNIX timestamp.
		/// </summary>
		/// <param name="dateTime">The date to convert</param>
		/// <returns>UNIX timestamp</returns>
		public static long ToUnix(this DateTime dateTime)
		{
			return Convert.ToInt64((dateTime.ToUniversalTime() - _unixEpoch).TotalSeconds);
		}

		/// <summary>
		/// Converts this UNIX timestamp into a <see cref="DateTime"/>.
		/// </summary>
		/// <param name="unixTimeStamp">The unix time stamp.</param>
		/// <returns>Date represented by this UNIX timestamp</returns>
		public static DateTime FromUnix(long unixTimeStamp)
		{
			return _unixEpoch.AddSeconds(unixTimeStamp).ToLocalTime();
		}

		/// <summary>
		/// Convert this DateTime into an RFC822-compliant string
		/// </summary>
		/// <param name="dateTime">The date to convert</param>
		/// <returns>String representing this date</returns>
		public static string ToRfc822String(this DateTime dateTime)
		{
			return dateTime.ToString("ddd',' d MMM yyyy HH':'mm':'ss") 
				+ " "
				+ dateTime.ToString("zzzz").Replace(":", "");
		}

		/// <summary>
		/// Converts this DateTime into a W3C string
		/// </summary>
		/// <param name="dateTime">The date to convert</param>
		/// <returns>String representing this date</returns>
		public static string ToW3CString(this DateTime dateTime)
		{
			return dateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
		}

		/// <summary>
		/// Calculate the number of years in the specified TimeSpan.
		/// </summary>
		/// <param name="timeSpan">Timespan</param>
		/// <returns>Years</returns>
		public static int Years(this TimeSpan timeSpan)
		{
			// I don't care about leap years here. YOLO.
			return (int)Math.Floor(timeSpan.Days / DAYS_IN_YEAR); 
		}
	}
}
