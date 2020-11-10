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
            var obj = new RangeYearMatcher(null, null);
            Assert.IsNotNull(obj, "Any year");
            Assert.IsNull(obj.Left);
            Assert.IsNull(obj.Right);
            
            obj = new RangeYearMatcher(2000, null);
            Assert.IsNotNull(obj, "Any year greater than or equal");
            Assert.AreEqual(2000, obj.Left.Value);
            Assert.IsNull(obj.Right);
            
            obj = new RangeYearMatcher(null, 2000);
            Assert.IsNotNull(obj,"Any year less than or equal");
            Assert.IsNull(obj.Left);
            Assert.AreEqual(2000, obj.Right.Value);
            
            obj = new RangeYearMatcher(2000, 2000);
            Assert.IsNotNull(obj, "Exact year");
            Assert.AreEqual(2000, obj.Left.Value);
            Assert.AreEqual(2000, obj.Right.Value);
            
            obj = new RangeYearMatcher(2000, 3000);
            Assert.IsNotNull(obj, "Finite range");
            Assert.AreEqual(2000, obj.Left.Value);
            Assert.AreEqual(3000, obj.Right.Value);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeYearMatcher(-100, null), "Non negative first year");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeYearMatcher(null, -100), "Non negative last year");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeYearMatcher(3000, 2000), "Invalid range");
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*",new RangeYearMatcher(null, null).ToString());
            Assert.AreEqual("2000..",new RangeYearMatcher(2000, null).ToString());
            Assert.AreEqual("..2000",new RangeYearMatcher(null, 2000).ToString());
            Assert.AreEqual("2000",new RangeYearMatcher(2000, 2000).ToString());
            Assert.AreEqual("2000..3000",new RangeYearMatcher(2000, 3000).ToString());
        }

        [TestMethod]
        public void OpenRangeCheckTests()
        {
            var obj = new RangeYearMatcher(null, null);
            Assert.IsTrue(obj.IsLeftOpenRange);
            Assert.IsTrue(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsTrue(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneYear);
            
            obj = new RangeYearMatcher(2000, null);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsTrue(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneYear);
            
            obj = new RangeYearMatcher(null, 2000);
            Assert.IsTrue(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneYear);
            
            obj = new RangeYearMatcher(2000, 2000);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsFalse(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsTrue(obj.IsClosedRange);
            Assert.IsTrue(obj.IsOneYear);
            
            obj = new RangeYearMatcher(2000, 3000);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsFalse(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsTrue(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneYear);
        }

        [TestMethod]
        public void MatchTests()
        {
            var dt1000 = new DateTime(1000, 1, 1);
            var dt2000 = new DateTime(2000, 1, 1);
            var dt2010 = new DateTime(2010, 1, 1);
            var dt3000 = new DateTime(3000, 1, 1);
            var dt9000 = new DateTime(9000, 1, 1);
            
            var obj = new RangeYearMatcher(null, null);
            Assert.IsTrue(obj.Match(dt1000));
            Assert.IsTrue(obj.Match(dt2000));
            Assert.IsTrue(obj.Match(dt2010));
            Assert.IsTrue(obj.Match(dt3000));
            Assert.IsTrue(obj.Match(dt9000));

            obj = new RangeYearMatcher(2000, null);
            Assert.IsFalse(obj.Match(dt1000));
            Assert.IsTrue(obj.Match(dt2000));
            Assert.IsTrue(obj.Match(dt2010));
            Assert.IsTrue(obj.Match(dt3000));
            Assert.IsTrue(obj.Match(dt9000));
            
            obj = new RangeYearMatcher(null, 2000);
            Assert.IsTrue(obj.Match(dt1000));
            Assert.IsTrue(obj.Match(dt2000));
            Assert.IsFalse(obj.Match(dt2010));
            Assert.IsFalse(obj.Match(dt3000));
            Assert.IsFalse(obj.Match(dt9000));
            
            obj = new RangeYearMatcher(2000, 2000);
            Assert.IsFalse(obj.Match(dt1000));
            Assert.IsTrue(obj.Match(dt2000));
            Assert.IsFalse(obj.Match(dt2010));
            Assert.IsFalse(obj.Match(dt3000));
            Assert.IsFalse(obj.Match(dt9000));
            
            obj = new RangeYearMatcher(2000, 3000);
            Assert.IsFalse(obj.Match(dt1000));
            Assert.IsTrue(obj.Match(dt2000));
            Assert.IsTrue(obj.Match(dt2010));
            Assert.IsTrue(obj.Match(dt3000));
            Assert.IsFalse(obj.Match(dt9000));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(RangeYearMatcher.TryParse("1..4", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.IsTrue(RangeYearMatcher.TryParse("*", out obj));
            Assert.IsNotNull(obj);
            Assert.IsNull(obj.Left);
            Assert.IsNull(obj.Right);
        }
    }
}