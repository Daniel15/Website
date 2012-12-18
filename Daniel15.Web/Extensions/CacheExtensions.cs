using System;
using System.Web.Caching;

namespace Daniel15.Web.Extensions
{
	public static class CacheExtensions
	{
		/// <summary>
		/// Get an item from the cache. If it doesn't exist, call the function to load it
		/// </summary>
		/// <typeparam name="T">Type of data</typeparam>
		/// <param name="cache">The cache.</param>
		/// <param name="key">The key.</param>
		/// <param name="absoluteExpiration">Time the item will expire, or <c>null</c> to use sliding expiration</param>
		/// <param name="slidingExpiration">Sliding expiration (see MSDN docs) or <c>null</c> to use absolute expiration</param>
		/// <param name="getData">Function to load data to cache. Called if data isn't in the cache, or is stale</param>
		/// <returns></returns>
		public static T GetOrInsert<T>(this Cache cache, string key, DateTime? absoluteExpiration, TimeSpan? slidingExpiration, Func<T> getData)
		{
			// Ensure cache exists - Return data directly otherwise
			if (cache == null)
				return getData();

			// Check for data in cache
			var data = (T)(cache[key] ?? default(T));

			if (object.Equals(data, default(T)))	// http://stackoverflow.com/questions/65351/null-or-default-comparsion-of-generic-argument-in-c-sharp
			{
				// Load data and save into cache
				data = getData();
				cache.Insert(key, data, null, absoluteExpiration ?? Cache.NoAbsoluteExpiration, slidingExpiration ?? Cache.NoSlidingExpiration);
			}

			return data;
		}
	}
}