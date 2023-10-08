using System;

namespace Daniel15.Web.Extensions
{
	/// <summary>
	/// Extension methods for strings.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Convert the first letter to uppercase (eg. "hello" -> "Hello")
		/// </summary>
		/// <param name="value">String</param>
		/// <returns>String with first letter as uppercase</returns>
		public static string FirstUpper(this string value)
		{
			return char.ToUpper(value[0]) + value.Substring(1);
		}

		/// <summary>
		/// Trims a substring from the start of this string. If the string does not start with the
		/// given substring, this is a no-op.
		/// </summary>
		/// <param name="value">String to trim</param>
		/// <param name="prefixToRemove">Substring to remove from the start</param>
		/// <returns>String with substring removed</returns>
		public static string TrimStart(this string value, string prefixToRemove)
		{
			return value.StartsWith(prefixToRemove)
				? value.Substring(prefixToRemove.Length)
				: value;
		}

		/// <summary>
		/// Try to parse the string as a URL. If it's not a valid URL, returns <c>null</c>.
		/// </summary>
		public static Uri ParseUriOrNull(this string maybeUri)
		{
			Uri.TryCreate(maybeUri, UriKind.Absolute, out var uri);
			return uri;
		}
	}
}
