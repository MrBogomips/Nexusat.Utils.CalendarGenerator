using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class ModuloDayOfMonthMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new ModuloDayOfMonthMatcher(null, null, 2);
            Assert.AreEqual(2, obj.Modulo);

            Assert.ThrowsException<ArgumentException>(() => new ModuloDayOfMonthMatcher(2, 2, 2),
                "Single year range invalid");
            Assert.ThrowsException<ArgumentException>(() => new ModuloDayOfMonthMatcher(2, 3, 1),
                "Invalid modulo");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ModuloDayOfMonthMatcher(1, 32, 2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ModuloDayOfMonthMatcher(32, null, 2));
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*%2",new ModuloDayOfMonthMatcher(null, null, 2).ToString());
            Assert.AreEqual("2..%3",new ModuloDayOfMonthMatcher(2, null, 3).ToString());
            Assert.AreEqual("..3%7",new ModuloDayOfMonthMatcher(null, 3, 7).ToString());
            Assert.AreEqual("*%2", new ModuloDayOfMonthMatcher(1, 31, 2).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var d1 = new DateTime(2000,1,1);
            var d2 = new DateTime(2000,1,3);
            var d3 = new DateTime(2000,1,6);
            var d4 = new DateTime(2000,1,9);

            var obj = new ModuloDayOfMonthMatcher(null, null, 3);
            Assert.IsFalse(obj.Match(d1));
            Assert.IsTrue(obj.Match(d2));
            Assert.IsTrue(obj.Match(d3));
            Assert.IsTrue(obj.Match(d4));
            
            obj = new ModuloDayOfMonthMatcher(4, null, 3);
            Assert.IsFalse(obj.Match(d1));
            Assert.IsFalse(obj.Match(d2));
            Assert.IsTrue(obj.Match(d3));
            Assert.IsTrue(obj.Match(d4));
            
            obj = new ModuloDayOfMonthMatcher(null, 6, 3);
            Assert.IsFalse(obj.Match(d1));
            Assert.IsTrue(obj.Match(d2));
            Assert.IsTrue(obj.Match(d3));
            Assert.IsFalse(obj.Match(d4));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(ModuloDayOfMonthMatcher.TryParse("1..4%2", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.AreEqual(2, obj.Modulo);
            Assert.IsTrue(ModuloDayOfMonthMatcher.TryParse("1..%3", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(31, obj.Right);
            Assert.AreEqual(3, obj.Modulo);
            Assert.IsTrue(ModuloDayOfMonthMatcher.TryParse("*%3", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(31, obj.Right);
            Assert.AreEqual(3, obj.Modulo);
        }
    }
}