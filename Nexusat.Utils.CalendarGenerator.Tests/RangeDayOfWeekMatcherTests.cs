using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class RangeDayOfWeekMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new RangeDayOfWeekMatcher(null, null);
            Assert.IsNotNull(obj, "Any week day");
            Assert.IsNull(obj.Left);
            Assert.IsNull(obj.Right);
                
            obj = new RangeDayOfWeekMatcher(1, null);
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left.Value);
            Assert.IsNull(obj.Right);
                
            obj = new RangeDayOfWeekMatcher(null, 7);
            Assert.IsNotNull(obj);
            Assert.IsNull(obj.Left);
            Assert.AreEqual(7, obj.Right.Value);
                
            obj = new RangeDayOfWeekMatcher(1, 1);
            Assert.IsNotNull(obj, "Exact day");
            Assert.AreEqual(1, obj.Left.Value);
            Assert.AreEqual(1, obj.Right.Value);
                
            obj = new RangeDayOfWeekMatcher(0, 7);
            Assert.IsNotNull(obj, "Finite range");
            Assert.AreEqual(0, obj.Left.Value);
            Assert.AreEqual(7, obj.Right.Value);
    
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeDayOfWeekMatcher(-1, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeDayOfWeekMatcher(null, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeDayOfWeekMatcher(8, null));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeDayOfWeekMatcher(null, 8));
        }
        
        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*",new RangeDayOfWeekMatcher(null, null).ToString());
            Assert.AreEqual("1..",new RangeDayOfWeekMatcher(1, null).ToString());
            Assert.AreEqual("..7",new RangeDayOfWeekMatcher(null, 7).ToString());
            Assert.AreEqual("5",new RangeDayOfWeekMatcher(5, 5).ToString());
            
            // Different ways to represent all the week days
            Assert.AreEqual("*",new RangeDayOfWeekMatcher(1, 7).ToString());
            Assert.AreEqual("*",new RangeDayOfWeekMatcher(0, 7).ToString());
            Assert.AreEqual("*",new RangeDayOfWeekMatcher(0, 6).ToString());
            
            Assert.AreEqual("7",new RangeDayOfWeekMatcher(7, null).ToString());
            Assert.AreEqual("0",new RangeDayOfWeekMatcher(null, 0).ToString());
        }
        
        [TestMethod]
        public void OpenRangeCheckTests()
        {
            var obj = new RangeDayOfWeekMatcher(null, null);
            Assert.IsTrue(obj.IsLeftOpenRange);
            Assert.IsTrue(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsTrue(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneWeekDay);
            
            obj = new RangeDayOfWeekMatcher(1, null);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsTrue(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneWeekDay);
            
            obj = new RangeDayOfWeekMatcher(null, 7);
            Assert.IsTrue(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneWeekDay);
            
            obj = new RangeDayOfWeekMatcher(5, 5);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsFalse(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsTrue(obj.IsClosedRange);
            Assert.IsTrue(obj.IsOneWeekDay);
            
            obj = new RangeDayOfWeekMatcher(0, 7);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsFalse(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsTrue(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneWeekDay);
        }

        [TestMethod]
        public void MatchTests()
        {
            var wd1 = new DateTime(2020, 6, 1); // Mon
            var wd2 = new DateTime(2020, 6, 2);
            var wd3 = new DateTime(2020, 6, 3);
            var wd4 = new DateTime(2020, 6, 4);
            var wd5 = new DateTime(2020, 6, 5);
            var wd6 = new DateTime(2020, 6, 6);
            var wd7 = new DateTime(2020, 6, 7); // Sun
            
            var obj = new RangeDayOfWeekMatcher(null, null);
            Assert.IsTrue(obj.Match(wd1));
            Assert.IsTrue(obj.Match(wd2));
            Assert.IsTrue(obj.Match(wd3));
            Assert.IsTrue(obj.Match(wd4));
            Assert.IsTrue(obj.Match(wd5));
            Assert.IsTrue(obj.Match(wd6));
            Assert.IsTrue(obj.Match(wd7));

            obj = new RangeDayOfWeekMatcher(2, null);
            Assert.IsFalse(obj.Match(wd1));
            Assert.IsTrue(obj.Match(wd2));
            Assert.IsTrue(obj.Match(wd3));
            Assert.IsTrue(obj.Match(wd4));
            Assert.IsTrue(obj.Match(wd5));
            Assert.IsTrue(obj.Match(wd6));
            Assert.IsTrue(obj.Match(wd7));  // Sun is evaluated also for 7
            
            obj = new RangeDayOfWeekMatcher(null, 2);
            Assert.IsTrue(obj.Match(wd1));
            Assert.IsTrue(obj.Match(wd2));
            Assert.IsFalse(obj.Match(wd3));
            Assert.IsFalse(obj.Match(wd4));
            Assert.IsFalse(obj.Match(wd5));
            Assert.IsFalse(obj.Match(wd6));
            Assert.IsTrue(obj.Match(wd7)); // Sun is evaluated also for 0
            
            obj = new RangeDayOfWeekMatcher(2, 2);
            Assert.IsFalse(obj.Match(wd1));
            Assert.IsTrue(obj.Match(wd2));
            Assert.IsFalse(obj.Match(wd3));
            Assert.IsFalse(obj.Match(wd4));
            Assert.IsFalse(obj.Match(wd5));
            Assert.IsFalse(obj.Match(wd6));
            Assert.IsFalse(obj.Match(wd7));
            
            obj = new RangeDayOfWeekMatcher(2, 4);
            Assert.IsFalse(obj.Match(wd1));
            Assert.IsTrue(obj.Match(wd2));
            Assert.IsTrue(obj.Match(wd3));
            Assert.IsTrue(obj.Match(wd4));
            Assert.IsFalse(obj.Match(wd5));
            Assert.IsFalse(obj.Match(wd6));
            Assert.IsFalse(obj.Match(wd7));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(RangeDayOfWeekMatcher.TryParse("1..4", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.IsTrue(RangeDayOfWeekMatcher.TryParse("*", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(0, obj.Left);
            Assert.AreEqual(7, obj.Right);
        }
    }
}