﻿using System.Collections.Generic;
using System.Web;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Extensions for building query strings.
	/// </summary>
	public static class QueryStringExtensions
	{
		/// <summary>
		/// Create a querystring based on the parameters in this dictionary.
		/// </summary>
		/// <param name="dictionary">Dictionary of parameters</param>
		/// <returns>Query string</returns>
		public static string ToQueryString(this IDictionary<string, object> dictionary)
		{
			// Create a new HttpValueCollection (it's internal so constructor can't be called)
			var output = HttpUtility.ParseQueryString(string.Empty);

			// Add all parameters to it
			foreach (var kvp in dictionary)
				output.Add(kvp.Key, kvp.Value.ToString());

			return output.ToString();
		}
	}
}