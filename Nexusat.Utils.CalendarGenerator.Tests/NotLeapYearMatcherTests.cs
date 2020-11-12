using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class NotLeapYearMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            _ = new NotLeapYearMatcher(null, null);
            _ = new NotLeapYearMatcher(2000, null);
            _ = new NotLeapYearMatcher(null, 2000);
            _ = new NotLeapYearMatcher(2000, 3000);
            Assert.ThrowsException<ArgumentException>(() => new NotLeapYearMatcher(2000, 2000));
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*/NotLeap",new NotLeapYearMatcher(null, null).ToString());
            Assert.AreEqual("2000../NotLeap",new NotLeapYearMatcher(2000, null).ToString());
            Assert.AreEqual("..2000/NotLeap",new NotLeapYearMatcher(null, 2000).ToString());
            Assert.AreEqual("2000..3000/NotLeap",new NotLeapYearMatcher(2000, 3000).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var obj = new NotLeapYearMatcher(null, null);
            var dt2000 = new DateTime(2000, 1, 1);
            var dt2001 = new DateTime(2001, 1, 1);
            var dt2020 = new DateTime(2020, 1, 1);
            var dt2100 = new DateTime(2100, 1, 1);
            Assert.IsFalse(obj.Match(dt2000));
            Assert.IsTrue(obj.Match(dt2001));
            Assert.IsFalse(obj.Match(dt2020));
            Assert.IsTrue(obj.Match(dt2100));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing object factory (valid)
            Assert.IsTrue(NotLeapYearMatcher.TryParse("*/NotLeap", out var obj));
            Assert.IsNotNull(obj);
            Assert.IsNull(obj.Left);
            Assert.IsNull(obj.Right);
            Assert.IsTrue(NotLeapYearMatcher.TryParse("1..100/NotLeap", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(100, obj.Right);
            
            // Testing object factory (invalid)
            Assert.IsFalse(NotLeapYearMatcher.TryParse("/NotLeap", out obj));
            Assert.IsNull(obj);
            Assert.IsFalse(NotLeapYearMatcher.TryParse("12/NotLeap", out obj));
            Assert.IsNull(obj);
            Assert.IsFalse(NotLeapYearMatcher.TryParse("12..12/NotLeap", out obj));
            Assert.IsNull(obj);
        }
    }
}