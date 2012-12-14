using System;
using System.Linq;
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
		/// Convert a short URL back to an ID
		/// </summary>
		/// <param name="alias">The short URL</param>
		/// <returns>ID represented by this short URL</returns>
		public int Extend(string alias)
		{
			// Ub3r-1337 LINQ madness!
			return alias
				.Reverse()
				// Get the index of each character, starting from the end
				.Select(currentChar => CHARACTERS.IndexOf(currentChar))
				// Calculate its value in the base-64 string
				.Select((charPos, index) => (int)Math.Pow(CHARACTERS.Length, index) * charPos)
				// Add all the character values together
				.Sum();

			// Normal, boring, non-leet version
			/*var output = 0;
			for (var i = 0; i < alias.Length; i++)
			{
				// Get i'th character from the end
				var currentChar = alias[alias.Length - i - 1];
				var charPos = CHARACTERS.IndexOf(currentChar);
				output += (int)Math.Pow(CHARACTERS.Length, i) * charPos;
			}

			return output;*/
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