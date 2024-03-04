namespace Daniel15.Web.Models;

/// <summary>
/// Represents an item on the social feed page
/// </summary>
public record SocialFeedItem(
	int Id,
	string Text,
	string? Description,
	string Url,
	int Date,
	string Type,
	string RelativeDate,
	string SubText
);
