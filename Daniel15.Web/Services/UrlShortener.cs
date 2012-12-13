using System;
using Daniel15.Web.Models.Blog;

namespace Daniel15.Web.Services
{
	public class UrlShortener : IUrlShortener
	{
		private const string CHARACTERS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz-_";

		/// <summary>
		/// Gets the short URL for this blog post
		/// </summary>
		/// <param name="post">Blog post</param>
		/// <returns>The short URL</returns>
		public string Shorten(PostSummaryModel post)
		{
			return Shorten(post.Id);
		}

		/// <summary>
		/// Gets the short URL alias for this ID
		/// </summary>
		/// <param name="id">The ID</param>
		/// <returns>The short URL alias</returns>
		public string Shorten(int id)
		{
			var numberTemp = id;
			string output = string.Empty;

			while (numberTemp > 0)
			{
				output = CHARACTERS[numberTemp % CHARACTERS.Length] + output;
				numberTemp /= CHARACTERS.Length;
			}

			return output;
		}
	}
}