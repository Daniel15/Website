using System.Text.Json.Serialization;

namespace Daniel15.Web.Models;

/// <summary>
/// Represents an item on the social feed page
/// </summary>
public record SocialFeedItem(
	int Id,
	string Text,
	string? Description,
	string Url,
	[property: JsonConverter(typeof(UnixTimestampDateTimeConverter))]
	DateTime Date,
	string Type,
	string RelativeDate,
	string SubText
);
