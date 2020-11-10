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

            // False month range that degenerate in a single month (december) wich is not a valid PeriodicMonthMatcher
            Assert.IsFalse(MonthMatcherParser.Instance.TryParse("12../2", out dateMatcher));
            Assert.IsNull(dateMatcher);

            Assert.IsFalse(MonthMatcherParser.Instance.TryParse("......", out dateMatcher));
            Assert.IsNull(dateMatcher);
        }

        [TestMethod]
        public void TryParseMultiTest()
        {
            var validParsingString = "*,*%2,1200../2,*/Leap,*/NotLeap";
            Assert.IsTrue(YearMatcherParser.Instance.TryParseMulti(validParsingString, out var yearMatchers));
            var matchers = new List<IYearMatcher>(yearMatchers);
            Assert.IsInstanceOfType(matchers[0], typeof(RangeYearMatcher));
            Assert.IsInstanceOfType(matchers[1], typeof(ModuloYearMatcher));
            Assert.IsInstanceOfType(matchers[2], typeof(PeriodicYearMatcher));
            Assert.IsInstanceOfType(matchers[3], typeof(LeapYearMatcher));
            Assert.IsInstanceOfType(matchers[4], typeof(NotLeapYearMatcher));
            
            var notValidParsingString = validParsingString + ",............";
            Assert.IsFalse(YearMatcherParser.Instance.TryParseMulti(notValidParsingString, out yearMatchers));
            Assert.IsNull(yearMatchers);
        }
    }
}