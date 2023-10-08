namespace Daniel15.Web.Models;

/// <summary>
/// Represents an item on the social feed page
/// </summary>
public record SocialFeedItem(
	string Id,
	string Text,
	string? Description,
	string Url,
	int Date,
	string Type,
	string RelativeDate,
	string SubText
);
