using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class RangeMonthMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new RangeMonthMatcher(null, null);
            Assert.IsNotNull(obj, "Any month");
            Assert.IsNull(obj.Left);
            Assert.IsNull(obj.Right);
                
            obj = new RangeMonthMatcher(1, null);
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left.Value);
            Assert.IsNull(obj.Right);
                
            obj = new RangeMonthMatcher(null, 12);
            Assert.IsNotNull(obj);
            Assert.IsNull(obj.Left);
            Assert.AreEqual(12, obj.Right.Value);
                
            obj = new RangeMonthMatcher(1, 1);
            Assert.IsNotNull(obj, "Exact month");
            Assert.AreEqual(1, obj.Left.Value);
            Assert.AreEqual(1, obj.Right.Value);
                
            obj = new RangeMonthMatcher(1, 12);
            Assert.IsNotNull(obj, "Finite range");
            Assert.AreEqual(1, obj.Left.Value);
            Assert.AreEqual(12, obj.Right.Value);
    
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeMonthMatcher(0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeMonthMatcher(null, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeMonthMatcher(13, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeMonthMatcher(null, 13));
        }
        
        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*",new RangeMonthMatcher(null, null).ToString());
            Assert.AreEqual("1..",new RangeMonthMatcher(1, null).ToString());
            Assert.AreEqual("..12",new RangeMonthMatcher(null, 12).ToString());
            Assert.AreEqual("11",new RangeMonthMatcher(11, 11).ToString());
            Assert.AreEqual("*",new RangeMonthMatcher(1, 12).ToString());
            Assert.AreEqual("12",new RangeMonthMatcher(12, null).ToString());
            Assert.AreEqual("1",new RangeMonthMatcher(null, 1).ToString());
        }
        
        [TestMethod]
        public void OpenRangeCheckTests()
        {
            var ryp = new RangeMonthMatcher(null, null);
            Assert.IsTrue(ryp.IsLeftOpenRange);
            Assert.IsTrue(ryp.IsRightOpenRange);
            Assert.IsTrue(ryp.IsOpenRange);
            Assert.IsTrue(ryp.IsLeftRightOpenRange);
            Assert.IsFalse(ryp.IsClosedRange);
            Assert.IsFalse(ryp.IsOneMonth);
            
            ryp = new RangeMonthMatcher(1, null);
            Assert.IsFalse(ryp.IsLeftOpenRange);
            Assert.IsTrue(ryp.IsRightOpenRange);
            Assert.IsTrue(ryp.IsOpenRange);
            Assert.IsFalse(ryp.IsLeftRightOpenRange);
            Assert.IsFalse(ryp.IsClosedRange);
            Assert.IsFalse(ryp.IsOneMonth);
            
            ryp = new RangeMonthMatcher(null, 12);
            Assert.IsTrue(ryp.IsLeftOpenRange);
            Assert.IsFalse(ryp.IsRightOpenRange);
            Assert.IsTrue(ryp.IsOpenRange);
            Assert.IsFalse(ryp.IsLeftRightOpenRange);
            Assert.IsFalse(ryp.IsClosedRange);
            Assert.IsFalse(ryp.IsOneMonth);
            
            ryp = new RangeMonthMatcher(11, 11);
            Assert.IsFalse(ryp.IsLeftOpenRange);
            Assert.IsFalse(ryp.IsRightOpenRange);
            Assert.IsFalse(ryp.IsOpenRange);
            Assert.IsFalse(ryp.IsLeftRightOpenRange);
            Assert.IsTrue(ryp.IsClosedRange);
            Assert.IsTrue(ryp.IsOneMonth);
            
            ryp = new RangeMonthMatcher(1, 12);
            Assert.IsFalse(ryp.IsLeftOpenRange);
            Assert.IsFalse(ryp.IsRightOpenRange);
            Assert.IsFalse(ryp.IsOpenRange);
            Assert.IsFalse(ryp.IsLeftRightOpenRange);
            Assert.IsTrue(ryp.IsClosedRange);
            Assert.IsFalse(ryp.IsOneMonth);
        }

        [TestMethod]
        public void MatchTests()
        {
            var m1 = new DateTime(2000, 1, 1);
            var m2 = new DateTime(2000, 2, 1);
            var m3 = new DateTime(2000, 3, 1);
            var m4 = new DateTime(2000, 4, 1);
            var m5 = new DateTime(2000, 5, 1);
            
            var obj = new RangeMonthMatcher(null, null);
            Assert.IsTrue(obj.Match(m1));
            Assert.IsTrue(obj.Match(m2));
            Assert.IsTrue(obj.Match(m3));
            Assert.IsTrue(obj.Match(m4));
            Assert.IsTrue(obj.Match(m5));

            obj = new RangeMonthMatcher(2, null);
            Assert.IsFalse(obj.Match(m1));
            Assert.IsTrue(obj.Match(m2));
            Assert.IsTrue(obj.Match(m3));
            Assert.IsTrue(obj.Match(m4));
            Assert.IsTrue(obj.Match(m5));
            
            obj = new RangeMonthMatcher(null, 2);
            Assert.IsTrue(obj.Match(m1));
            Assert.IsTrue(obj.Match(m2));
            Assert.IsFalse(obj.Match(m3));
            Assert.IsFalse(obj.Match(m4));
            Assert.IsFalse(obj.Match(m5));
            
            obj = new RangeMonthMatcher(2, 2);
            Assert.IsFalse(obj.Match(m1));
            Assert.IsTrue(obj.Match(m2));
            Assert.IsFalse(obj.Match(m3));
            Assert.IsFalse(obj.Match(m4));
            Assert.IsFalse(obj.Match(m5));
            
            obj = new RangeMonthMatcher(2, 4);
            Assert.IsFalse(obj.Match(m1));
            Assert.IsTrue(obj.Match(m2));
            Assert.IsTrue(obj.Match(m3));
            Assert.IsTrue(obj.Match(m4));
            Assert.IsFalse(obj.Match(m5));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(RangeMonthMatcher.TryParse("1..4", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.IsTrue(RangeMonthMatcher.TryParse("*", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(12, obj.Right);
        }
    }
}