using Daniel15.BusinessLayer.Services;
using Daniel15.Web.Services;
using NUnit.Framework;

namespace Daniel15.Web.Tests
{
	[TestFixture]
	public class UrlShortenerTests
	{
		private UrlShortener _urlShortener;

		[SetUp]
		public void Init()
		{
			_urlShortener = new UrlShortener();
		}

		[TestCase(1, Result = "1")]
		[TestCase(10, Result = "A")]
		[TestCase(100, Result = "1a")]
		[TestCase(1000, Result = "Fe")]
		public string Shorten(int id)
		{
			return _urlShortener.Shorten(id);
		}

		[TestCase("1", Result = 1)]
		[TestCase("10", Result = 64)]
		[TestCase("AA", Result = 650)]
		[TestCase("aa", Result = 2340)]
		public int Extend(string alias)
		{
			return _urlShortener.Extend(alias);
		}
	}
}
