using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Daniel15.Shared.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="HttpClient"/> and related classes.
	/// </summary>
    public static class HttpRequestExtensions
    {
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
