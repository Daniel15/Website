using System;
using Daniel15.Shared.Extensions;
using NUnit.Framework;

namespace Daniel15.Web.Tests.Extensions
{
	[TestFixture]
	public class DateExtensionsTests
	{
		[TestCase(1, Result = "st")]
		[TestCase(2, Result = "nd")]
		[TestCase(3, Result = "rd")]
		[TestCase(4, Result = "th")]
		[TestCase(10, Result = "th")]
		[TestCase(11, Result = "th")]
		[TestCase(12, Result = "th")]
		[TestCase(13, Result = "th")]
		[TestCase(14, Result = "th")]
		[TestCase(20, Result = "th")]
		[TestCase(21, Result = "st")]
		[TestCase(22, Result = "nd")]
		[TestCase(100, Result = "th")]
		[TestCase(101, Result = "st")]
		[TestCase(111, Result = "th")]
		public string Ordinal(int num)
		{
			return DateExtensions.Ordinal(num);
		}

		[Test]
		public void ToUnix()
		{
			Assert.AreEqual(0, DateExtensions.ToUnix(new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc)));
			Assert.AreEqual(1356507658, DateExtensions.ToUnix(new DateTime(year: 2012, month: 12, day: 26, hour: 7, minute: 40, second: 58, kind: DateTimeKind.Utc)));
		}

		[Test]
		public void FromUnix()
		{
			Assert.AreEqual(new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc).ToLocalTime(), DateExtensions.FromUnix(0));
			Assert.AreEqual(new DateTime(year: 2012, month: 12, day: 26, hour: 7, minute: 40, second: 58, kind: DateTimeKind.Utc).ToLocalTime(), DateExtensions.FromUnix(1356507658));
		}
	}
}
