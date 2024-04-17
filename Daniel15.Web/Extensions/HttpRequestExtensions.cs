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
		/// Decodes the content as a dynamic JSON object.
		/// </summary>
		/// <param name="httpContent">Content to decode</param>
		/// <returns>Dynamic object</returns>
		public static async Task<JObject> ReadAsDynamicJsonAsync(this HttpContent httpContent)
		{
			var responseText = await httpContent.ReadAsStringAsync();
			return JObject.Parse(responseText);
		}
	}
}
