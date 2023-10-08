using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="HttpRequest"/>.
	/// </summary>
	public static class HttpRequestExtensions
	{
		/// <summary>
		/// Determines if the specified request should redirect to FeedBurner. This is the case when
		/// a user accesses an RSS feed directly. Allows explicit access to the feed by adding a
		/// feedburner_override GET parameter. 
		/// </summary>
		/// <param name="request">The request</param>
		/// <returns><c>true</c> if this request can redirect to FeedBurner</returns>
		public static bool ShouldRedirectToFeedburner(this HttpRequest request)
		{
			var userAgent = request.Headers["User-Agent"].Count > 0
				? request.Headers["User-Agent"][0].ToLower()
				: string.Empty;
			return
				!userAgent.Contains("feedburner")
				&& !userAgent.Contains("feedvalidator")
				&& !request.Query.ContainsKey("feedburner_override");
		}

		/// <summary>
		/// Send a GET to the sepecified URI and return the response body as a JArray (dynamic 
		/// JSON array)
		/// </summary>
		/// <param name="client">HttpClient</param>
		/// <param name="requestUri">URI to send request to</param>
		/// <returns>Array</returns>
		public static async Task<JArray> GetJArrayAsync(this HttpClient client, string requestUri)
		{
			var responseText = await client.GetStringAsync(requestUri);
			return JArray.Parse(responseText);
		}

		/// <summary>
		/// Send a GET to the sepecified URI and return the response body as a dynamic JSON object.
		/// </summary>
		/// <param name="client">HttpClient</param>
		/// <param name="requestUri">URI to send request to</param>
		/// <returns>Dynamic object</returns>
		public static async Task<JObject> GetDynamicJsonAsync(this HttpClient client, string requestUri)
		{
			var responseText = await client.GetStringAsync(requestUri);
			return JObject.Parse(responseText);
		}

		/// <summary>
		/// Send a GET to the sepecified URI and decodes the response as JSON
		/// </summary>
		/// <typeparam name="T">Type to deserialize into</typeparam>
		/// <param name="client">HttpClient</param>
		/// <param name="requestUri">URI to send request to</param>
		/// <returns>Instance of <typeparamref name="T"/></returns>
		public static async Task<T> GetJsonAsync<T>(this HttpClient client, string requestUri)
		{
			var responseText = await client.GetStringAsync(requestUri);
			return JsonConvert.DeserializeObject<T>(responseText);
		}

		/// <summary>
		/// Decodes the content as a JArray (dynamic JSON array)
		/// </summary>
		/// <param name="httpContent">Content to decode</param>
		/// <returns>Array</returns>
		public static async Task<JArray> ReadAsJArrayAsync(this HttpContent httpContent)
		{
			var responseText = await httpContent.ReadAsStringAsync();
			return JArray.Parse(responseText);
		}


		/// <summary>
		/// Decodes the content as a dynamic JSON object.
		/// </summary>
		/// <param name="httpContent">Content to decode</param>
		/// <returns>Dynamic object</returns>
		public static async Task<JObject> ReadAsDynamicJsonAsync(this HttpContent httpContent)
		{
			var responseText = await httpContent.ReadAsStringAsync();
			return JObject.Parse(responseText);
		}

		/// <summary>
		/// Decodes the content as JSON
		/// </summary>
		/// <typeparam name="T">Type to deserialize into</typeparam>
		/// <param name="httpContent">Content to decode</param>
		/// <returns>Instance of <typeparamref name="T"/></returns>
		public static async Task<T> ReadAsJsonAsync<T>(this HttpContent httpContent)
		{
			var responseText = await httpContent.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(responseText);
		}
	}
}
