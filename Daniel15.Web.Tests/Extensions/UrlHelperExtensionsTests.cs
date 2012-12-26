
using Daniel15.Web.Extensions;
using NUnit.Framework;

namespace Daniel15.Web.Tests.Extensions
{
	[TestFixture]
	public class UrlHelperExtensionsTests
	{
		[TestCase("/test", Result = "http://localhost/test")]
		[TestCase("/test/foo?1=23", Result = "http://localhost/test/foo?1=23")]
		public string Absolute(string uri)
		{
			var mocks = new Mocks();
			return UrlHelperExtensions.Absolute(mocks.UrlHelper, uri);
		}
	}
}
