using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class NonLeapYearTests
    {
        [TestMethod]
        public void CtorTests()
        {
            _ = new NonLeapYearMatcher(null, null);
            _ = new NonLeapYearMatcher(2000, null);
            _ = new NonLeapYearMatcher(null, 2000);
            _ = new NonLeapYearMatcher(2000, 3000);
            Assert.ThrowsException<ArgumentException>(() => new NonLeapYearMatcher(2000, 2000));
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*/NotLeap",new NonLeapYearMatcher(null, null).ToString());
            Assert.AreEqual("2000../NotLeap",new NonLeapYearMatcher(2000, null).ToString());
            Assert.AreEqual("..2000/NotLeap",new NonLeapYearMatcher(null, 2000).ToString());
            Assert.AreEqual("2000..3000/NotLeap",new NonLeapYearMatcher(2000, 3000).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var lym = new NonLeapYearMatcher(null, null);
            var dt2000 = new DateTime(2000, 1, 1);
            var dt2001 = new DateTime(2001, 1, 1);
            var dt2020 = new DateTime(2020, 1, 1);
            var dt2100 = new DateTime(2100, 1, 1);
            Assert.IsFalse(lym.Match(dt2000));
            Assert.IsTrue(lym.Match(dt2001));
            Assert.IsFalse(lym.Match(dt2020));
            Assert.IsTrue(lym.Match(dt2100));
        }
    }
}