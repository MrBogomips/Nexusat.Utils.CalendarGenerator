using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class RangeNumberMatcherTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var obj = new RangeNumberMatcher(null, null);
            Assert.IsNotNull(obj, "Any number");
            Assert.IsNull(obj.Left);
            Assert.IsNull(obj.Right);
            
            obj = new RangeNumberMatcher(2000, null);
            Assert.IsNotNull(obj, "Any year greater than or equal");
            Assert.AreEqual(2000, obj.Left.Value);
            Assert.IsNull(obj.Right);
            
            obj = new RangeNumberMatcher(null, 2000);
            Assert.IsNotNull(obj,"Any year less than or equal");
            Assert.IsNull(obj.Left);
            Assert.AreEqual(2000, obj.Right.Value);
            
            obj = new RangeNumberMatcher(2000, 2000);
            Assert.IsNotNull(obj, "Exact year");
            Assert.AreEqual(2000, obj.Left.Value);
            Assert.AreEqual(2000, obj.Right.Value);
            
            obj = new RangeNumberMatcher(2000, 3000);
            Assert.IsNotNull(obj, "Finite range");
            Assert.AreEqual(2000, obj.Left.Value);
            Assert.AreEqual(3000, obj.Right.Value);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeNumberMatcher(-100, null), "Non negative first year");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeNumberMatcher(null, -100), "Non negative last year");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RangeNumberMatcher(3000, 2000), "Invalid range");
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*",new RangeNumberMatcher(null, null).ToString());
            Assert.AreEqual("2000..",new RangeNumberMatcher(2000, null).ToString());
            Assert.AreEqual("..2000",new RangeNumberMatcher(null, 2000).ToString());
            Assert.AreEqual("2000",new RangeNumberMatcher(2000, 2000).ToString());
            Assert.AreEqual("2000..3000",new RangeNumberMatcher(2000, 3000).ToString());
        }

        [TestMethod]
        public void OpenRangeCheckTests()
        {
            var obj = new RangeNumberMatcher(null, null);
            Assert.IsTrue(obj.IsLeftOpenRange);
            Assert.IsTrue(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsTrue(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneValue);
            
            obj = new RangeNumberMatcher(2000, null);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsTrue(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneValue);
            
            obj = new RangeNumberMatcher(null, 2000);
            Assert.IsTrue(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsTrue(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsFalse(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneValue);
            
            obj = new RangeNumberMatcher(2000, 2000);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsFalse(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsTrue(obj.IsClosedRange);
            Assert.IsTrue(obj.IsOneValue);
            
            obj = new RangeNumberMatcher(2000, 3000);
            Assert.IsFalse(obj.IsLeftOpenRange);
            Assert.IsFalse(obj.IsRightOpenRange);
            Assert.IsFalse(obj.IsOpenRange);
            Assert.IsFalse(obj.IsLeftRightOpenRange);
            Assert.IsTrue(obj.IsClosedRange);
            Assert.IsFalse(obj.IsOneValue);
        }

        [TestMethod]
        public void MatchTests()
        {
            var n1000 = 1000;
            var n2000 = 2000;
            var n2010 = 2010;
            var n3000 = 3000;
            var n9000 = 9000;
            
            var obj = new RangeNumberMatcher(null, null);
            Assert.IsTrue(obj.Match(n1000));
            Assert.IsTrue(obj.Match(n2000));
            Assert.IsTrue(obj.Match(n2010));
            Assert.IsTrue(obj.Match(n3000));
            Assert.IsTrue(obj.Match(n9000));

            obj = new RangeNumberMatcher(2000, null);
            Assert.IsFalse(obj.Match(n1000));
            Assert.IsTrue(obj.Match(n2000));
            Assert.IsTrue(obj.Match(n2010));
            Assert.IsTrue(obj.Match(n3000));
            Assert.IsTrue(obj.Match(n9000));
            
            obj = new RangeNumberMatcher(null, 2000);
            Assert.IsTrue(obj.Match(n1000));
            Assert.IsTrue(obj.Match(n2000));
            Assert.IsFalse(obj.Match(n2010));
            Assert.IsFalse(obj.Match(n3000));
            Assert.IsFalse(obj.Match(n9000));
            
            obj = new RangeNumberMatcher(2000, 2000);
            Assert.IsFalse(obj.Match(n1000));
            Assert.IsTrue(obj.Match(n2000));
            Assert.IsFalse(obj.Match(n2010));
            Assert.IsFalse(obj.Match(n3000));
            Assert.IsFalse(obj.Match(n9000));
            
            obj = new RangeNumberMatcher(2000, 3000);
            Assert.IsFalse(obj.Match(n1000));
            Assert.IsTrue(obj.Match(n2000));
            Assert.IsTrue(obj.Match(n2010));
            Assert.IsTrue(obj.Match(n3000));
            Assert.IsFalse(obj.Match(n9000));
        }

        [TestMethod]
        public void DeconstructTests()
        {
            var obj = new RangeNumberMatcher(1, 2);

            var (l, r) = obj;
            
            Assert.AreEqual(1, l);
            Assert.AreEqual(2, r);
        }

        [TestMethod]
        public void TryParseTests()
        {
            int? left, right;
            
            // Testing invalid patterns
            Assert.IsFalse(RangeNumberMatcher.TryParse("..", null, null, null, null, out left, out right));
            Assert.IsFalse(RangeNumberMatcher.TryParse("*..", null, null, null, null, out left, out right));
            Assert.IsFalse(RangeNumberMatcher.TryParse("*..*",null, null, null, null,  out left, out right));
            Assert.IsFalse(RangeNumberMatcher.TryParse("..*",null, null, null, null,  out left, out right));
            
            // Testing valid patterns
            Assert.IsTrue(RangeNumberMatcher.TryParse("*", null, null, null, null, out left, out right));
            Assert.IsNull(left);
            Assert.IsNull(right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..", null, null, null, null, out left, out right));
            Assert.AreEqual(1, left);
            Assert.IsNull(right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("..2", null, null, null, null, out left, out right));
            Assert.IsNull(left);
            Assert.AreEqual(2, right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..2", null, null, null, null, out left, out right));
            Assert.AreEqual(1, left);
            Assert.AreEqual(2, right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("4", null, null, null, null, out left, out right));
            Assert.AreEqual(4, left);
            Assert.AreEqual(4, right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("4..4", null, null, null, null, out left, out right));
            Assert.AreEqual(4, left);
            Assert.AreEqual(4, right);
            
            // Range checked
            Assert.IsFalse(RangeNumberMatcher.TryParse("4..", 5, null, null, null, out left, out right));
            Assert.IsFalse(RangeNumberMatcher.TryParse("6..", null, 5, null, null, out left, out right));
            Assert.IsFalse(RangeNumberMatcher.TryParse("..4", null, null, 5, null, out left, out right));
            Assert.IsFalse(RangeNumberMatcher.TryParse("..6", null, null, null, 5, out left, out right));
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("*", 1, null, null, null, out left, out right));
            Assert.AreEqual(1, left);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("*", null, null, null, 5, out left, out right));
            Assert.AreEqual(5, right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("*", 1, null, null, 5, out left, out right));
            Assert.AreEqual(1, left);
            Assert.AreEqual(5, right);
            
            // This is a false right-open interval
            Assert.IsTrue(RangeNumberMatcher.TryParse("12..", null, null, null, 12, out left, out right));
            Assert.AreEqual(12, left);
            Assert.AreEqual(12, right);
            
            // This is a false left-open interval
            Assert.IsTrue(RangeNumberMatcher.TryParse("..1", 1, null, null, null, out left, out right));
            Assert.AreEqual(1, left);
            Assert.AreEqual(1, right);
            
            // Testing object factory
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..4", out var obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("*", out obj));
            Assert.IsNotNull(obj);
            Assert.IsNull(obj.Left);
            Assert.IsNull(obj.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.IsNull(obj.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("..4", out obj));
            Assert.IsNotNull(obj);
            Assert.IsNull(obj.Left);
            Assert.AreEqual(4, obj.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..1", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(1, obj.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("1", out obj));
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Left);
            Assert.AreEqual(1, obj.Right);
        }
    }
}