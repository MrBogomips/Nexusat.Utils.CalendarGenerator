using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class PeriodicMonthMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new PeriodicMonthMatcher(1, null, 2);
            Assert.AreEqual(2, obj.Period);

            Assert.ThrowsException<ArgumentException>(() => new PeriodicMonthMatcher(2, 2, 2),
                "Single year range invalid");
            Assert.ThrowsException<ArgumentException>(() => new PeriodicMonthMatcher(1, 12, 1),
                "Invalid period");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PeriodicMonthMatcher(1, 13, 2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PeriodicMonthMatcher(13, null, 2));
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("1../3",new PeriodicMonthMatcher(1, null, 3).ToString());
            Assert.AreEqual("*/7",new PeriodicMonthMatcher(1, 12, 7).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var m1 = new DateTime(2000,1,1);
            var m2 = new DateTime(2000,2,1);
            var m3 = new DateTime(2000,3,1);
            var m4 = new DateTime(2000,4,1);

            var obj = new PeriodicMonthMatcher(1, null, 2);
            Assert.IsTrue(obj.Match(m1));
            Assert.IsFalse(obj.Match(m2));
            Assert.IsTrue(obj.Match(m3));
            Assert.IsFalse(obj.Match(m4));
            obj = new PeriodicMonthMatcher(2, null, 2);
            Assert.IsFalse(obj.Match(m1));
            Assert.IsTrue(obj.Match(m2));
            Assert.IsFalse(obj.Match(m3));
            Assert.IsTrue(obj.Match(m4));
            obj = new PeriodicMonthMatcher(3, null, 2);
            Assert.IsFalse(obj.Match(m1));
            Assert.IsFalse(obj.Match(m2));
            Assert.IsTrue(obj.Match(m3));
            Assert.IsFalse(obj.Match(m4));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(PeriodicMonthMatcher.TryParse("1..4/2", out var dateMatcher));
            Assert.IsNotNull(dateMatcher);
            Assert.AreEqual(1, dateMatcher.Left);
            Assert.AreEqual(4, dateMatcher.Right);
            Assert.AreEqual(2, dateMatcher.Period);
            Assert.IsTrue(PeriodicMonthMatcher.TryParse("1../3", out dateMatcher));
            Assert.IsNotNull(dateMatcher);
            Assert.AreEqual(1, dateMatcher.Left);
            Assert.AreEqual(12, dateMatcher.Right);
            Assert.AreEqual(3, dateMatcher.Period);
        }
    }
}