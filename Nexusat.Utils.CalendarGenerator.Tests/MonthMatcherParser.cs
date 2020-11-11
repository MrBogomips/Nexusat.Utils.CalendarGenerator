using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class MonthMatcherParserTests
    {
        [TestMethod]
        public void TryParseTest()
        {
            Assert.IsTrue(MonthMatcherParser.Instance.TryParse("*", out var dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(RangeMonthMatcher));

            Assert.IsTrue(MonthMatcherParser.Instance.TryParse("*%2", out dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(ModuloMonthMatcher));
            
            Assert.IsTrue(MonthMatcherParser.Instance.TryParse("*/2", out dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(PeriodicMonthMatcher));

            // False month range that degenerate in a single month (december) wich is not a valid PeriodicMonthMatcher
            Assert.IsFalse(MonthMatcherParser.Instance.TryParse("12../2", out dateMatcher));
            Assert.IsNull(dateMatcher);

            Assert.IsFalse(MonthMatcherParser.Instance.TryParse("......", out dateMatcher));
            Assert.IsNull(dateMatcher);
        }

        [TestMethod]
        public void TryParseMultiTest()
        {
            var validParsingString = "*,*%2,*/2";
            Assert.IsTrue(MonthMatcherParser.Instance.TryParseMulti(validParsingString, out var obj));
            var matchers = new List<IMonthMatcher>(obj);
            Assert.IsInstanceOfType(matchers[0], typeof(RangeMonthMatcher));
            Assert.IsInstanceOfType(matchers[1], typeof(ModuloMonthMatcher));
            Assert.IsInstanceOfType(matchers[2], typeof(PeriodicMonthMatcher));
            
            var notValidParsingString = validParsingString + ",............";
            Assert.IsFalse(MonthMatcherParser.Instance.TryParseMulti(notValidParsingString, out obj));
            Assert.IsNull(obj);
        }
    }
}