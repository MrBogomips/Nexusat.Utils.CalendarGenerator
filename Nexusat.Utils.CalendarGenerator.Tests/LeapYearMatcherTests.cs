using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class LeapYearMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            _ = new LeapYearMatcher(null, null);
            _ = new LeapYearMatcher(2000, null);
            _ = new LeapYearMatcher(null, 2000);
            _ = new LeapYearMatcher(2000, 3000);
            Assert.ThrowsException<ArgumentException>(() => new LeapYearMatcher(2000, 2000));
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*/Leap",new LeapYearMatcher(null, null).ToString());
            Assert.AreEqual("2000../Leap",new LeapYearMatcher(2000, null).ToString());
            Assert.AreEqual("..2000/Leap",new LeapYearMatcher(null, 2000).ToString());
            Assert.AreEqual("2000..3000/Leap",new LeapYearMatcher(2000, 3000).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var lym = new LeapYearMatcher(null, null);
            var dt2000 = new DateTime(2000, 1, 1);
            var dt2001 = new DateTime(2001, 1, 1);
            var dt2020 = new DateTime(2020, 1, 1);
            var dt2100 = new DateTime(2100, 1, 1);
            Assert.IsTrue(lym.Match(dt2000));
            Assert.IsFalse(lym.Match(dt2001));
            Assert.IsTrue(lym.Match(dt2020));
            Assert.IsFalse(lym.Match(dt2100));
        }

        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory (valid)
            Assert.IsTrue(LeapYearMatcher.TryParse("*/Leap", out var lym));
            Assert.IsNotNull(lym);
            Assert.IsNull(lym.Left);
            Assert.IsNull(lym.Right);
            Assert.IsTrue(LeapYearMatcher.TryParse("1..100/Leap", out lym));
            Assert.IsNotNull(lym);
            Assert.AreEqual(1, lym.Left);
            Assert.AreEqual(100, lym.Right);
            
            // Testing object factory (invalid)
            Assert.IsFalse(LeapYearMatcher.TryParse("/Leap", out lym));
            Assert.IsNull(lym);
            Assert.IsFalse(LeapYearMatcher.TryParse("12/Leap", out lym));
            Assert.IsNull(lym);
            Assert.IsFalse(LeapYearMatcher.TryParse("12..12/Leap", out lym));
            Assert.IsNull(lym);
        }
    }
}