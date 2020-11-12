using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class RangeDayOfMonthMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new RangeDayOfMonthMatcher(null, null);
            Assert.IsNotNull(obj, "Any day");
            Assert.IsNull(obj.Left);
            Assert.IsNull(obj.Right);
                
            obj = new RangeDayOfMonthMatcher(1, null);
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left.Value);
            Assert.IsNull(obj.Right);
                
            obj = new RangeDayOfMonthMatcher(null, 31);
            Assert.IsNotNull(obj);
            Assert.IsNull(obj.Left);
            Assert.AreEqual(31, obj.Right.Value);
                
            obj = new RangeDayOfMonthMatcher(1, 1);
            Assert.IsNotNull(obj, "Exact day");
            Assert.AreEqual(1, obj.Left.Value);
            Assert.AreEqual(1, obj.Right.Value);
                
            obj = new RangeDayOfMonthMatcher(1, 31);
            Assert.IsNotNull(obj, "Finite range");
            Assert.AreEqual(1, obj.Left.Value);
            Assert.AreEqual(31, obj.Right.Value);
    
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeDayOfMonthMatcher(0, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeDayOfMonthMatcher(null, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeDayOfMonthMatcher(32, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeDayOfMonthMatcher(null, 32));
        }
        
        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*",new RangeDayOfMonthMatcher(null, null).ToString());
            Assert.AreEqual("1..",new RangeDayOfMonthMatcher(1, null).ToString());
            Assert.AreEqual("..31",new RangeDayOfMonthMatcher(null, 31).ToString());
            Assert.AreEqual("15",new RangeDayOfMonthMatcher(15, 15).ToString());
            Assert.AreEqual("*",new RangeDayOfMonthMatcher(1, 31).ToString());
            Assert.AreEqual("31",new RangeDayOfMonthMatcher(31, null).ToString());
            Assert.AreEqual("1",new RangeDayOfMonthMatcher(null, 1).ToString());
        }
        
        [TestMethod]
        public void OpenRangeCheckTests()
        {
            var obj = new RangeDayOfMonthMatcher(null, null);
            Assert.IsTrue(obj.IsLeftOpenRange);
            Assert.IsTrue(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsTrue(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneDay);
            
            obj = new RangeDayOfMonthMatcher(1, null);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsTrue(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneDay);
            
            obj = new RangeDayOfMonthMatcher(null, 31);
            Assert.IsTrue(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneDay);
            
            obj = new RangeDayOfMonthMatcher(11, 11);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsFalse(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsTrue(obj.IsClosedRange);
            Assert.IsTrue(obj.IsOneDay);
            
            obj = new RangeDayOfMonthMatcher(1, 31);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsFalse(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsTrue(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneDay);
        }

        [TestMethod]
        public void MatchTests()
        {
            var d1 = new DateTime(2000, 1, 1);
            var d2 = new DateTime(2000, 1, 2);
            var d3 = new DateTime(2000, 1, 3);
            var d4 = new DateTime(2000, 1, 4);
            var d5 = new DateTime(2000, 1, 5);
            
            var obj = new RangeDayOfMonthMatcher(null, null);
            Assert.IsTrue(obj.Match(d1));
            Assert.IsTrue(obj.Match(d2));
            Assert.IsTrue(obj.Match(d3));
            Assert.IsTrue(obj.Match(d4));
            Assert.IsTrue(obj.Match(d5));

            obj = new RangeDayOfMonthMatcher(2, null);
            Assert.IsFalse(obj.Match(d1));
            Assert.IsTrue(obj.Match(d2));
            Assert.IsTrue(obj.Match(d3));
            Assert.IsTrue(obj.Match(d4));
            Assert.IsTrue(obj.Match(d5));
            
            obj = new RangeDayOfMonthMatcher(null, 2);
            Assert.IsTrue(obj.Match(d1));
            Assert.IsTrue(obj.Match(d2));
            Assert.IsFalse(obj.Match(d3));
            Assert.IsFalse(obj.Match(d4));
            Assert.IsFalse(obj.Match(d5));
            
            obj = new RangeDayOfMonthMatcher(2, 2);
            Assert.IsFalse(obj.Match(d1));
            Assert.IsTrue(obj.Match(d2));
            Assert.IsFalse(obj.Match(d3));
            Assert.IsFalse(obj.Match(d4));
            Assert.IsFalse(obj.Match(d5));
            
            obj = new RangeDayOfMonthMatcher(2, 4);
            Assert.IsFalse(obj.Match(d1));
            Assert.IsTrue(obj.Match(d2));
            Assert.IsTrue(obj.Match(d3));
            Assert.IsTrue(obj.Match(d4));
            Assert.IsFalse(obj.Match(d5));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(RangeDayOfMonthMatcher.TryParse("1..4", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.IsTrue(RangeDayOfMonthMatcher.TryParse("*", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(31, obj.Right);
        }
    }
}