using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class NotLeapYearTests
    {
        [TestMethod]
        public void CtorTests()
        {
            _ = new NotLeapYearMatcher(null, null);
            _ = new NotLeapYearMatcher(2000, null);
            _ = new NotLeapYearMatcher(null, 2000);
            _ = new NotLeapYearMatcher(2000, 3000);
            Assert.ThrowsException<ArgumentException>(() => new NotLeapYearMatcher(2000, 2000));
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*/NotLeap",new NotLeapYearMatcher(null, null).ToString());
            Assert.AreEqual("2000../NotLeap",new NotLeapYearMatcher(2000, null).ToString());
            Assert.AreEqual("..2000/NotLeap",new NotLeapYearMatcher(null, 2000).ToString());
            Assert.AreEqual("2000..3000/NotLeap",new NotLeapYearMatcher(2000, 3000).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var lym = new NotLeapYearMatcher(null, null);
            var dt2000 = new DateTime(2000, 1, 1);
            var dt2001 = new DateTime(2001, 1, 1);
            var dt2020 = new DateTime(2020, 1, 1);
            var dt2100 = new DateTime(2100, 1, 1);
            Assert.IsFalse(lym.Match(dt2000));
            Assert.IsTrue(lym.Match(dt2001));
            Assert.IsFalse(lym.Match(dt2020));
            Assert.IsTrue(lym.Match(dt2100));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory (valid)
            Assert.IsTrue(NotLeapYearMatcher.TryParse("*/NotLeap", out var nlym));
            Assert.IsNotNull(nlym);
            Assert.IsNull(nlym.Left);
            Assert.IsNull(nlym.Right);
            Assert.IsTrue(NotLeapYearMatcher.TryParse("1..100/NotLeap", out nlym));
            Assert.IsNotNull(nlym);
            Assert.AreEqual(1, nlym.Left);
            Assert.AreEqual(100, nlym.Right);
            
            // Testing object factory (invalid)
            Assert.IsFalse(NotLeapYearMatcher.TryParse("/NotLeap", out nlym));
            Assert.IsNull(nlym);
            Assert.IsFalse(NotLeapYearMatcher.TryParse("12/NotLeap", out nlym));
            Assert.IsNull(nlym);
            Assert.IsFalse(NotLeapYearMatcher.TryParse("12..12/NotLeap", out nlym));
            Assert.IsNull(nlym);
        }
    }
}