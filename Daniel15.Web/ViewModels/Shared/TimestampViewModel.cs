namespace Daniel15.Web.ViewModels.Shared;

public readonly record struct TimestampViewModel(
	DateTime DateTime,
	bool ShowYear = true,
	// https://schema.org/docs/gs.html#microdata_itemprop
	string? ItemProp = null
);
