using System;
namespace Daniel15.Shared.Extensions
{
	/// <summary>
	/// Extension methods relating to enums
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Parse this string as an enum value of the specified type.
		/// Strongly-typed version of <see cref="Enum.Parse(Type, string)"/>.
		/// </summary>
		/// <typeparam name="T">Type of the enum</typeparam>
		/// <param name="value">String to parse</param>
		/// <returns>Enum value</returns>
		public static T ParseEnum<T>(this string value)
		{
			return (T) Enum.Parse(typeof (T), value);
		}
	}
}
