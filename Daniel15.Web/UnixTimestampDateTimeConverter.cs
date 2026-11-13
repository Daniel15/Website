using System.Text.Json;
using System.Text.Json.Serialization;
using Daniel15.Web.Extensions;

namespace Daniel15.Web;

/// <summary>
/// Converts a Unix timestamp to a <see cref="DateTime"/>.
/// </summary>
public class UnixTimestampDateTimeConverter : JsonConverter<DateTime>
{
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var timestamp = reader.TokenType switch
		{
			JsonTokenType.Number => reader.GetInt64(),
			// HAX: PHP version of SocialFeeds returns timestamp as a string instead of as a number.
			// TODO: Clean this up once it's rewritten in C#.
			JsonTokenType.String => long.Parse(reader.GetString()),
			_ => throw new InvalidOperationException(
				$"Expected a number, but got {reader.TokenType}!"
			)
		};
		return DateExtensions.FromUnix(timestamp);
	}

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		writer.WriteNumberValue(value.ToUnix());
	}
}
