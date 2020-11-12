using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class PeriodicNumberMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new PeriodicNumberMatcher(2000, null, 2);
            Assert.AreEqual(2, obj.Period);

            Assert.ThrowsException<ArgumentException>(() => new PeriodicNumberMatcher(2000, 2000, 2),
                "Single year range invalid");
            Assert.ThrowsException<ArgumentException>(() => new PeriodicNumberMatcher(2000, 3000, 1),
                "Invalid period");
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("2000../3",new PeriodicNumberMatcher(2000, null, 3).ToString());
            Assert.AreEqual("2000..3000/7",new PeriodicNumberMatcher(2000, 3000, 7).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var n2000 = 2000;
            var n2001 = 2001;
            var n2002 = 2002;
            var n2003 = 2003;

            var obj = new PeriodicNumberMatcher(2000, null, 2);
            Assert.IsTrue(obj.Match(n2000));
            Assert.IsFalse(obj.Match(n2001));
            Assert.IsTrue(obj.Match(n2002));
            Assert.IsFalse(obj.Match(n2003));
            obj = new PeriodicNumberMatcher(2001, null, 2);
            Assert.IsFalse(obj.Match(n2000));
            Assert.IsTrue(obj.Match(n2001));
            Assert.IsFalse(obj.Match(n2002));
            Assert.IsTrue(obj.Match(n2003));
            obj = new PeriodicNumberMatcher(2002, null, 2);
            Assert.IsFalse(obj.Match(n2000));
            Assert.IsFalse(obj.Match(n2001));
            Assert.IsTrue(obj.Match(n2002));
            Assert.IsFalse(obj.Match(n2003));
        }
        
        [TestMethod]
        public void TryParseTests()
        {
            // Testing invalid patterns
            Assert.IsFalse(PeriodicNumberMatcher.TryParse("*/5",null, null, null, null, out var left, out var right, out var  period));
            Assert.IsFalse(PeriodicNumberMatcher.TryParse("..120/5", null, null, null, null,out left, out right, out period));
            Assert.IsFalse(PeriodicNumberMatcher.TryParse("120../1", null, null, null, null,out left, out right, out period));
            Assert.IsFalse(PeriodicNumberMatcher.TryParse("120..120/2",null, null, null, null, out left, out right, out period));
            Assert.IsFalse(PeriodicNumberMatcher.TryParse("120/2", null, null, null, null,out left, out right, out period));
            
            // Testing valid patterns
            Assert.IsTrue(PeriodicNumberMatcher.TryParse("120../2",null, null, null, null, out left, out right, out period));
            Assert.AreEqual(120, left);
            Assert.IsNull(right);
            Assert.AreEqual(2, period);
            Assert.IsTrue(PeriodicNumberMatcher.TryParse("120..240/2",null, null, null, null, out left, out right, out period));
            Assert.AreEqual(120, left);
            Assert.AreEqual(240, right);
            Assert.AreEqual(2, period);

            // Testing object factory (invalid patterns)
            Assert.IsFalse(PeriodicNumberMatcher.TryParse("120/3", out var obj));
            Assert.IsNull(obj);
            
            Assert.IsFalse(PeriodicNumberMatcher.TryParse("120..120/3", out obj));
            Assert.IsNull(obj);
            
            Assert.IsFalse(PeriodicNumberMatcher.TryParse("100../1", out obj));
            Assert.IsNull(obj);
            
            // Testing object factory (valid patterns)
            Assert.IsTrue(PeriodicNumberMatcher.TryParse("100../2", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(100, obj.Left);
            Assert.IsNull(obj.Right);
            
            Assert.IsTrue(PeriodicNumberMatcher.TryParse("100..200/2", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(100, obj.Left);
            Assert.AreEqual(200, obj.Right);
        }
    }
}