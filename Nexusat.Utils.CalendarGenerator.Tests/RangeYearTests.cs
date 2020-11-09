using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class RangeYearTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var rym = new CronDayRule.RangeYearMatcher(null, null);
            Assert.IsNotNull(rym, "Any year");
            Assert.IsNull(rym.FirstYear);
            Assert.IsNull(rym.LastYear);
            
            rym = new CronDayRule.RangeYearMatcher(2000, null);
            Assert.IsNotNull(rym, "Any year greater than or equal");
            Assert.AreEqual(2000, rym.FirstYear.Value);
            Assert.IsNull(rym.LastYear);
            
            rym = new CronDayRule.RangeYearMatcher(null, 2000);
            Assert.IsNotNull(rym,"Any year less than or equal");
            Assert.IsNull(rym.FirstYear);
            Assert.AreEqual(2000, rym.LastYear.Value);
            
            rym = new CronDayRule.RangeYearMatcher(2000, 2000);
            Assert.IsNotNull(rym, "Exact year");
            Assert.AreEqual(2000, rym.FirstYear.Value);
            Assert.AreEqual(2000, rym.LastYear.Value);
            
            rym = new CronDayRule.RangeYearMatcher(2000, 3000);
            Assert.IsNotNull(rym, "Finite range");
            Assert.AreEqual(2000, rym.FirstYear.Value);
            Assert.AreEqual(3000, rym.LastYear.Value);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new CronDayRule.RangeYearMatcher(-100, null), "Non negative first year");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new CronDayRule.RangeYearMatcher(null, -100), "Non negative last year");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new CronDayRule.RangeYearMatcher(3000, 2000), "Invalid range");
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*",new CronDayRule.RangeYearMatcher(null, null).ToString());
            Assert.AreEqual("2000..",new CronDayRule.RangeYearMatcher(2000, null).ToString());
            Assert.AreEqual("..2000",new CronDayRule.RangeYearMatcher(null, 2000).ToString());
            Assert.AreEqual("2000",new CronDayRule.RangeYearMatcher(2000, 2000).ToString());
            Assert.AreEqual("2000..3000",new CronDayRule.RangeYearMatcher(2000, 3000).ToString());
        }

        [TestMethod]
        public void OpenRangeCheckTests()
        {
            var ryp = new CronDayRule.RangeYearMatcher(null, null);
            Assert.IsTrue(ryp.IsLeftOpenRange);
            Assert.IsTrue(ryp.IsRightOpenRange);
            Assert.IsTrue(ryp.IsOpenRange);
            Assert.IsTrue(ryp.IsLeftRightOpenRange);
            Assert.IsFalse(ryp.IsClosedRange);
            Assert.IsFalse(ryp.IsOneYear);
            
            ryp = new CronDayRule.RangeYearMatcher(2000, null);
            Assert.IsFalse(ryp.IsLeftOpenRange);
            Assert.IsTrue(ryp.IsRightOpenRange);
            Assert.IsTrue(ryp.IsOpenRange);
            Assert.IsFalse(ryp.IsLeftRightOpenRange);
            Assert.IsFalse(ryp.IsClosedRange);
            Assert.IsFalse(ryp.IsOneYear);
            
            ryp = new CronDayRule.RangeYearMatcher(null, 2000);
            Assert.IsTrue(ryp.IsLeftOpenRange);
            Assert.IsFalse(ryp.IsRightOpenRange);
            Assert.IsTrue(ryp.IsOpenRange);
            Assert.IsFalse(ryp.IsLeftRightOpenRange);
            Assert.IsFalse(ryp.IsClosedRange);
            Assert.IsFalse(ryp.IsOneYear);
            
            ryp = new CronDayRule.RangeYearMatcher(2000, 2000);
            Assert.IsFalse(ryp.IsLeftOpenRange);
            Assert.IsFalse(ryp.IsRightOpenRange);
            Assert.IsFalse(ryp.IsOpenRange);
            Assert.IsFalse(ryp.IsLeftRightOpenRange);
            Assert.IsTrue(ryp.IsClosedRange);
            Assert.IsTrue(ryp.IsOneYear);
            
            ryp = new CronDayRule.RangeYearMatcher(2000, 3000);
            Assert.IsFalse(ryp.IsLeftOpenRange);
            Assert.IsFalse(ryp.IsRightOpenRange);
            Assert.IsFalse(ryp.IsOpenRange);
            Assert.IsFalse(ryp.IsLeftRightOpenRange);
            Assert.IsTrue(ryp.IsClosedRange);
            Assert.IsFalse(ryp.IsOneYear);
        }

        [TestMethod]
        public void MatchTests()
        {
            var dt1000 = new DateTime(1000, 1, 1);
            var dt2000 = new DateTime(2000, 1, 1);
            var dt2010 = new DateTime(2010, 1, 1);
            var dt3000 = new DateTime(3000, 1, 1);
            var dt9000 = new DateTime(9000, 1, 1);
            
            var ryp = new CronDayRule.RangeYearMatcher(null, null);
            Assert.IsTrue(ryp.Match(dt1000));
            Assert.IsTrue(ryp.Match(dt2000));
            Assert.IsTrue(ryp.Match(dt2010));
            Assert.IsTrue(ryp.Match(dt3000));
            Assert.IsTrue(ryp.Match(dt9000));

            ryp = new CronDayRule.RangeYearMatcher(2000, null);
            Assert.IsFalse(ryp.Match(dt1000));
            Assert.IsTrue(ryp.Match(dt2000));
            Assert.IsTrue(ryp.Match(dt2010));
            Assert.IsTrue(ryp.Match(dt3000));
            Assert.IsTrue(ryp.Match(dt9000));
            
            ryp = new CronDayRule.RangeYearMatcher(null, 2000);
            Assert.IsTrue(ryp.Match(dt1000));
            Assert.IsTrue(ryp.Match(dt2000));
            Assert.IsFalse(ryp.Match(dt2010));
            Assert.IsFalse(ryp.Match(dt3000));
            Assert.IsFalse(ryp.Match(dt9000));
            
            ryp = new CronDayRule.RangeYearMatcher(2000, 2000);
            Assert.IsFalse(ryp.Match(dt1000));
            Assert.IsTrue(ryp.Match(dt2000));
            Assert.IsFalse(ryp.Match(dt2010));
            Assert.IsFalse(ryp.Match(dt3000));
            Assert.IsFalse(ryp.Match(dt9000));
            
            ryp = new CronDayRule.RangeYearMatcher(2000, 3000);
            Assert.IsFalse(ryp.Match(dt1000));
            Assert.IsTrue(ryp.Match(dt2000));
            Assert.IsTrue(ryp.Match(dt2010));
            Assert.IsTrue(ryp.Match(dt3000));
            Assert.IsFalse(ryp.Match(dt9000));
        }
    }
}