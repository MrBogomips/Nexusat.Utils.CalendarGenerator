using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class DayOfMonthMonthMatcherParserTests
    {
        [TestClass]
        public class MonthMatcherParserTests
        {
            [TestMethod]
            public void TryParseTest()
            {
                Assert.IsTrue(DayOfMonthMonthMatcherParser.Instance.TryParse("*", out var dateMatcher));
                Assert.IsInstanceOfType(dateMatcher, typeof(RangeDayOfMonthMatcher));

                Assert.IsTrue(DayOfMonthMonthMatcherParser.Instance.TryParse("*%2", out dateMatcher));
                Assert.IsInstanceOfType(dateMatcher, typeof(ModuloDayOfMonthMatcher));
                
                Assert.IsTrue(DayOfMonthMonthMatcherParser.Instance.TryParse("*/2", out dateMatcher));
                Assert.IsInstanceOfType(dateMatcher, typeof(PeriodicDayOfMonthMatcher));

                // False month range that degenerate in a single month (december) wich is not a valid PeriodicMonthMatcher
                Assert.IsFalse(DayOfMonthMonthMatcherParser.Instance.TryParse("31../2", out dateMatcher));
                Assert.IsNull(dateMatcher);

                Assert.IsFalse(DayOfMonthMonthMatcherParser.Instance.TryParse("......", out dateMatcher));
                Assert.IsNull(dateMatcher);
            }

            [TestMethod]
            public void TryParseMultiTest()
            {
                var validParsingString = "*,*%2,*/2";
                Assert.IsTrue(DayOfMonthMonthMatcherParser.Instance.TryParseMulti(validParsingString, out var obj));
                var matchers = new List<IDayOfMonthMatcher>(obj);
                Assert.IsInstanceOfType(matchers[0], typeof(RangeDayOfMonthMatcher));
                Assert.IsInstanceOfType(matchers[1], typeof(ModuloDayOfMonthMatcher));
                Assert.IsInstanceOfType(matchers[2], typeof(PeriodicDayOfMonthMatcher));
            
                var notValidParsingString = validParsingString + ",............";
                Assert.IsFalse(DayOfMonthMonthMatcherParser.Instance.TryParseMulti(notValidParsingString, out obj));
                Assert.IsNull(obj);
            }
        }
    }
}