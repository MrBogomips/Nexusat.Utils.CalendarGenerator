using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class ModuloYearMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new ModuloYearMatcher(null, null, 2);
            Assert.AreEqual(2, obj.Modulo);

            Assert.ThrowsException<ArgumentException>(() => new ModuloYearMatcher(2000, 2000, 2),
                "Single year range invalid");
            Assert.ThrowsException<ArgumentException>(() => new ModuloYearMatcher(2000, 3000, 1),
                "Invalid modulo");
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*%2",new ModuloYearMatcher(null, null, 2).ToString());
            Assert.AreEqual("2000..%3",new ModuloYearMatcher(2000, null, 3).ToString());
            Assert.AreEqual("..3000%7",new ModuloYearMatcher(null, 3000, 7).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var dt2000 = new DateTime(2000,1,1);
            var dt2001 = new DateTime(2001,1,1);
            var dt3000 = new DateTime(3000,1,1);
            var dt3003 = new DateTime(3003,1,1);

            var obj = new ModuloYearMatcher(null, null, 3);
            Assert.IsFalse(obj.Match(dt2000));
            Assert.IsTrue(obj.Match(dt2001));
            Assert.IsTrue(obj.Match(dt3000));
            Assert.IsTrue(obj.Match(dt3003));
            
            obj = new ModuloYearMatcher(2500, null, 3);
            Assert.IsFalse(obj.Match(dt2000));
            Assert.IsFalse(obj.Match(dt2001));
            Assert.IsTrue(obj.Match(dt3000));
            Assert.IsTrue(obj.Match(dt3003));
            
            obj = new ModuloYearMatcher(null, 3000, 3);
            Assert.IsFalse(obj.Match(dt2000));
            Assert.IsTrue(obj.Match(dt2001));
            Assert.IsTrue(obj.Match(dt3000));
            Assert.IsFalse(obj.Match(dt3003));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory
            Assert.IsTrue(ModuloYearMatcher.TryParse("1..4%2", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.AreEqual(2, obj.Modulo);
            Assert.IsTrue(ModuloYearMatcher.TryParse("1..%3", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.IsNull(obj.Right);
            Assert.AreEqual(3, obj.Modulo);
            Assert.IsTrue(ModuloYearMatcher.TryParse("*%3", out obj));
            Assert.IsNotNull(obj);
            Assert.IsNull(obj.Left);
            Assert.IsNull(obj.Right);
            Assert.AreEqual(3, obj.Modulo);
        }
    }
}