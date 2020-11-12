using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class YearMatcherParserTests
    {
        [TestMethod]
        public void TryParseTest()
        {
            Assert.IsTrue(YearMatcherParser.Instance.TryParse("*", out var dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(RangeYearMatcher));

            Assert.IsTrue(YearMatcherParser.Instance.TryParse("*%2", out dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(ModuloYearMatcher));

            Assert.IsTrue(YearMatcherParser.Instance.TryParse("1200../2", out dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(PeriodicYearMatcher));

            Assert.IsTrue(YearMatcherParser.Instance.TryParse("*/Leap", out dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(LeapYearMatcher));

            Assert.IsTrue(YearMatcherParser.Instance.TryParse("*/NotLeap", out dateMatcher));
            Assert.IsInstanceOfType(dateMatcher, typeof(NotLeapYearMatcher));

            Assert.IsFalse(YearMatcherParser.Instance.TryParse("......", out dateMatcher));
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