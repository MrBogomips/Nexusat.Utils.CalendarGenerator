using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class YearMatcherParserTests
    {
        [TestMethod]
        public void TryParseTest()
        {
            Assert.IsTrue(YearMatcherParser.Instance.TryParse("*", out var yearMatcher));
            Assert.IsInstanceOfType(yearMatcher, typeof(RangeYearMatcher));

            Assert.IsTrue(YearMatcherParser.Instance.TryParse("*%2", out yearMatcher));
            Assert.IsInstanceOfType(yearMatcher, typeof(ModuloYearMatcher));

            Assert.IsTrue(YearMatcherParser.Instance.TryParse("1200../2", out yearMatcher));
            Assert.IsInstanceOfType(yearMatcher, typeof(PeriodicYearMatcher));

            Assert.IsTrue(YearMatcherParser.Instance.TryParse("*/Leap", out yearMatcher));
            Assert.IsInstanceOfType(yearMatcher, typeof(LeapYearMatcher));

            Assert.IsTrue(YearMatcherParser.Instance.TryParse("*/NotLeap", out yearMatcher));
            Assert.IsInstanceOfType(yearMatcher, typeof(NotLeapYearMatcher));

            Assert.IsFalse(YearMatcherParser.Instance.TryParse("......", out yearMatcher));
            Assert.IsNull(yearMatcher);
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