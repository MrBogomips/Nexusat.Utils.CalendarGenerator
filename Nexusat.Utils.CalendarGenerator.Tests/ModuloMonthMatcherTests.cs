using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class ModuloMonthMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new ModuloMonthMatcher(null, null, 2);
            Assert.AreEqual(2, obj.Modulo);

            Assert.ThrowsException<ArgumentException>(() => new ModuloMonthMatcher(2, 2, 2),
                "Single year range invalid");
            Assert.ThrowsException<ArgumentException>(() => new ModuloMonthMatcher(2, 3, 1),
                "Invalid modulo");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ModuloMonthMatcher(1, 13, 2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ModuloMonthMatcher(13, null, 2));
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*%2",new ModuloMonthMatcher(null, null, 2).ToString());
            Assert.AreEqual("2..%3",new ModuloMonthMatcher(2, null, 3).ToString());
            Assert.AreEqual("..3%7",new ModuloMonthMatcher(null, 3, 7).ToString());
            Assert.AreEqual("*%2", new ModuloMonthMatcher(1, 12, 2).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var m1 = new DateTime(2000,1,1);
            var m2 = new DateTime(2000,3,1);
            var m3 = new DateTime(2000,6,1);
            var m4 = new DateTime(2000,9,1);

            var obj = new ModuloMonthMatcher(null, null, 3);
            Assert.IsFalse(obj.Match(m1));
            Assert.IsTrue(obj.Match(m2));
            Assert.IsTrue(obj.Match(m3));
            Assert.IsTrue(obj.Match(m4));
            
            obj = new ModuloMonthMatcher(4, null, 3);
            Assert.IsFalse(obj.Match(m1));
            Assert.IsFalse(obj.Match(m2));
            Assert.IsTrue(obj.Match(m3));
            Assert.IsTrue(obj.Match(m4));
            
            obj = new ModuloMonthMatcher(null, 6, 3);
            Assert.IsFalse(obj.Match(m1));
            Assert.IsTrue(obj.Match(m2));
            Assert.IsTrue(obj.Match(m3));
            Assert.IsFalse(obj.Match(m4));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(ModuloMonthMatcher.TryParse("1..4%2", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.AreEqual(2, obj.Modulo);
            Assert.IsTrue(ModuloMonthMatcher.TryParse("1..%3", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(12, obj.Right);
            Assert.AreEqual(3, obj.Modulo);
            Assert.IsTrue(ModuloMonthMatcher.TryParse("*%3", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(12, obj.Right);
            Assert.AreEqual(3, obj.Modulo);
        }
    }
}