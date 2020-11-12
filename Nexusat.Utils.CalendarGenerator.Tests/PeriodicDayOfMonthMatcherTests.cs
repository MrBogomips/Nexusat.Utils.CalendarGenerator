using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class PeriodicDayOfMonthMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new PeriodicDayOfMonthMatcher(1, null, 2);
            Assert.AreEqual(2, obj.Period);

            Assert.ThrowsException<ArgumentException>(() => new PeriodicDayOfMonthMatcher(2, 2, 2),
                "Single day of month range invalid");
            Assert.ThrowsException<ArgumentException>(() => new PeriodicDayOfMonthMatcher(1, 12, 1),
                "Invalid period");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PeriodicDayOfMonthMatcher(1, 32, 2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PeriodicDayOfMonthMatcher(32, null, 2));
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("1../3",new PeriodicDayOfMonthMatcher(1, null, 3).ToString());
            Assert.AreEqual("*/7",new PeriodicDayOfMonthMatcher(1, 31, 7).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var d1 = new DateTime(2000,1,1);
            var d2 = new DateTime(2000,1,2);
            var d3 = new DateTime(2000,1,3);
            var d4 = new DateTime(2000,1,4);

            var obj = new PeriodicDayOfMonthMatcher(1, null, 2);
            Assert.IsTrue(obj.Match(d1));
            Assert.IsFalse(obj.Match(d2));
            Assert.IsTrue(obj.Match(d3));
            Assert.IsFalse(obj.Match(d4));
            obj = new PeriodicDayOfMonthMatcher(2, null, 2);
            Assert.IsFalse(obj.Match(d1));
            Assert.IsTrue(obj.Match(d2));
            Assert.IsFalse(obj.Match(d3));
            Assert.IsTrue(obj.Match(d4));
            obj = new PeriodicDayOfMonthMatcher(3, null, 2);
            Assert.IsFalse(obj.Match(d1));
            Assert.IsFalse(obj.Match(d2));
            Assert.IsTrue(obj.Match(d3));
            Assert.IsFalse(obj.Match(d4));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(PeriodicDayOfMonthMatcher.TryParse("1..4/2", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.AreEqual(2, obj.Period);
            Assert.IsTrue(PeriodicDayOfMonthMatcher.TryParse("1../3", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(31, obj.Right);
            Assert.AreEqual(3, obj.Period);
        }
    }
}