using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class DayOfMonthMonthMatcherParserTests
    {
        [TestMethod]
        public void TryParseTest()
        {
            Assert.IsTrue(DayOfMonthMatcherParser.Instance.TryParse("*", out var dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(RangeDayOfMonthMatcher));

            Assert.IsTrue(DayOfMonthMatcherParser.Instance.TryParse("*%2", out dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(ModuloDayOfMonthMatcher));

            Assert.IsTrue(DayOfMonthMatcherParser.Instance.TryParse("*/2", out dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(PeriodicDayOfMonthMatcher));

            // False month range that degenerate in a single month (december) wich is not a valid PeriodicMonthMatcher
            Assert.IsFalse(DayOfMonthMatcherParser.Instance.TryParse("31../2", out dateMatcher));
            Assert.IsNull(dateMatcher);

            Assert.IsFalse(DayOfMonthMatcherParser.Instance.TryParse("......", out dateMatcher));
            Assert.IsNull(dateMatcher);
        }

        [TestMethod]
        public void TryParseMultiTest()
        {
            var validParsingString = "*,*%2,*/2";
            Assert.IsTrue(DayOfMonthMatcherParser.Instance.TryParseMulti(validParsingString, out var obj));
            var matchers = new List<IDayOfMonthMatcher>(obj);
            Assert.IsInstanceOfType(matchers[0], typeof(RangeDayOfMonthMatcher));
            Assert.IsInstanceOfType(matchers[1], typeof(ModuloDayOfMonthMatcher));
            Assert.IsInstanceOfType(matchers[2], typeof(PeriodicDayOfMonthMatcher));

            var notValidParsingString = validParsingString + ",............";
            Assert.IsFalse(DayOfMonthMatcherParser.Instance.TryParseMulti(notValidParsingString, out obj));
            Assert.IsNull(obj);
        }
    }
}