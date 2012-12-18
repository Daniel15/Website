using System;

namespace Daniel15.Web.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Reverses this string
		/// </summary>
		/// <param name="input">String to reverse</param>
		/// <returns>Reversed version of the string</returns>
		public static string Reverse(this string input)
		{
			var chars = input.ToCharArray();
			Array.Reverse(chars);
			return new string(chars);
		}
	}
}