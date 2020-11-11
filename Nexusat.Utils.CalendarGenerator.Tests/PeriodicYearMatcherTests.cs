using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class PeriodicYearMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new PeriodicYearMatcher(2000, null, 2);
            Assert.AreEqual(2, obj.Period);

            Assert.ThrowsException<ArgumentException>(() => new PeriodicYearMatcher(2000, 2000, 2),
                "Single year range invalid");
            Assert.ThrowsException<ArgumentException>(() => new PeriodicYearMatcher(2000, 3000, 1),
                "Invalid period");
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("2000../3",new PeriodicYearMatcher(2000, null, 3).ToString());
            Assert.AreEqual("2000..3000/7",new PeriodicYearMatcher(2000, 3000, 7).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var dt2000 = new DateTime(2000,1,1);
            var dt2001 = new DateTime(2001,1,1);
            var dt2002 = new DateTime(2002,1,1);
            var dt2003 = new DateTime(2003,1,1);

            var obj = new PeriodicYearMatcher(2000, null, 2);
            Assert.IsTrue(obj.Match(dt2000));
            Assert.IsFalse(obj.Match(dt2001));
            Assert.IsTrue(obj.Match(dt2002));
            Assert.IsFalse(obj.Match(dt2003));
            obj = new PeriodicYearMatcher(2001, null, 2);
            Assert.IsFalse(obj.Match(dt2000));
            Assert.IsTrue(obj.Match(dt2001));
            Assert.IsFalse(obj.Match(dt2002));
            Assert.IsTrue(obj.Match(dt2003));
            obj = new PeriodicYearMatcher(2002, null, 2);
            Assert.IsFalse(obj.Match(dt2000));
            Assert.IsFalse(obj.Match(dt2001));
            Assert.IsTrue(obj.Match(dt2002));
            Assert.IsFalse(obj.Match(dt2003));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(PeriodicYearMatcher.TryParse("1..4/2", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.AreEqual(2, obj.Period);
            Assert.IsTrue(PeriodicYearMatcher.TryParse("1../3", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.IsNull(obj.Right);
            Assert.AreEqual(3, obj.Period);
        }
    }
}