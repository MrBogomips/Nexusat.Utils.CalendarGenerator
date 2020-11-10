using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class RangeNumberTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var rym = new RangeNumberMatcher(null, null);
            Assert.IsNotNull(rym, "Any number");
            Assert.IsNull(rym.Left);
            Assert.IsNull(rym.Right);
            
            rym = new RangeNumberMatcher(2000, null);
            Assert.IsNotNull(rym, "Any year greater than or equal");
            Assert.AreEqual(2000, rym.Left.Value);
            Assert.IsNull(rym.Right);
            
            rym = new RangeNumberMatcher(null, 2000);
            Assert.IsNotNull(rym,"Any year less than or equal");
            Assert.IsNull(rym.Left);
            Assert.AreEqual(2000, rym.Right.Value);
            
            rym = new RangeNumberMatcher(2000, 2000);
            Assert.IsNotNull(rym, "Exact year");
            Assert.AreEqual(2000, rym.Left.Value);
            Assert.AreEqual(2000, rym.Right.Value);
            
            rym = new RangeNumberMatcher(2000, 3000);
            Assert.IsNotNull(rym, "Finite range");
            Assert.AreEqual(2000, rym.Left.Value);
            Assert.AreEqual(3000, rym.Right.Value);

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
            var rnm = new RangeNumberMatcher(null, null);
            Assert.IsTrue(rnm.IsLeftOpenRange);
            Assert.IsTrue(rnm.IsRightOpenRange);
            Assert.IsTrue(rnm.IsOpenRange);
            Assert.IsTrue(rnm.IsLeftRightOpenRange);
            Assert.IsFalse(rnm.IsClosedRange);
            Assert.IsFalse(rnm.IsOneValue);
            
            rnm = new RangeNumberMatcher(2000, null);
            Assert.IsFalse(rnm.IsLeftOpenRange);
            Assert.IsTrue(rnm.IsRightOpenRange);
            Assert.IsTrue(rnm.IsOpenRange);
            Assert.IsFalse(rnm.IsLeftRightOpenRange);
            Assert.IsFalse(rnm.IsClosedRange);
            Assert.IsFalse(rnm.IsOneValue);
            
            rnm = new RangeNumberMatcher(null, 2000);
            Assert.IsTrue(rnm.IsLeftOpenRange);
            Assert.IsFalse(rnm.IsRightOpenRange);
            Assert.IsTrue(rnm.IsOpenRange);
            Assert.IsFalse(rnm.IsLeftRightOpenRange);
            Assert.IsFalse(rnm.IsClosedRange);
            Assert.IsFalse(rnm.IsOneValue);
            
            rnm = new RangeNumberMatcher(2000, 2000);
            Assert.IsFalse(rnm.IsLeftOpenRange);
            Assert.IsFalse(rnm.IsRightOpenRange);
            Assert.IsFalse(rnm.IsOpenRange);
            Assert.IsFalse(rnm.IsLeftRightOpenRange);
            Assert.IsTrue(rnm.IsClosedRange);
            Assert.IsTrue(rnm.IsOneValue);
            
            rnm = new RangeNumberMatcher(2000, 3000);
            Assert.IsFalse(rnm.IsLeftOpenRange);
            Assert.IsFalse(rnm.IsRightOpenRange);
            Assert.IsFalse(rnm.IsOpenRange);
            Assert.IsFalse(rnm.IsLeftRightOpenRange);
            Assert.IsTrue(rnm.IsClosedRange);
            Assert.IsFalse(rnm.IsOneValue);
        }

        [TestMethod]
        public void MatchTests()
        {
            var n1000 = 1000;
            var n2000 = 2000;
            var n2010 = 2010;
            var n3000 = 3000;
            var n9000 = 9000;
            
            var ryp = new RangeNumberMatcher(null, null);
            Assert.IsTrue(ryp.Match(n1000));
            Assert.IsTrue(ryp.Match(n2000));
            Assert.IsTrue(ryp.Match(n2010));
            Assert.IsTrue(ryp.Match(n3000));
            Assert.IsTrue(ryp.Match(n9000));

            ryp = new RangeNumberMatcher(2000, null);
            Assert.IsFalse(ryp.Match(n1000));
            Assert.IsTrue(ryp.Match(n2000));
            Assert.IsTrue(ryp.Match(n2010));
            Assert.IsTrue(ryp.Match(n3000));
            Assert.IsTrue(ryp.Match(n9000));
            
            ryp = new RangeNumberMatcher(null, 2000);
            Assert.IsTrue(ryp.Match(n1000));
            Assert.IsTrue(ryp.Match(n2000));
            Assert.IsFalse(ryp.Match(n2010));
            Assert.IsFalse(ryp.Match(n3000));
            Assert.IsFalse(ryp.Match(n9000));
            
            ryp = new RangeNumberMatcher(2000, 2000);
            Assert.IsFalse(ryp.Match(n1000));
            Assert.IsTrue(ryp.Match(n2000));
            Assert.IsFalse(ryp.Match(n2010));
            Assert.IsFalse(ryp.Match(n3000));
            Assert.IsFalse(ryp.Match(n9000));
            
            ryp = new RangeNumberMatcher(2000, 3000);
            Assert.IsFalse(ryp.Match(n1000));
            Assert.IsTrue(ryp.Match(n2000));
            Assert.IsTrue(ryp.Match(n2010));
            Assert.IsTrue(ryp.Match(n3000));
            Assert.IsFalse(ryp.Match(n9000));
        }

        [TestMethod]
        public void DeconstructTests()
        {
            var rnm = new RangeNumberMatcher(1, 2);

            var (l, r) = rnm;
            
            Assert.AreEqual(1, l);
            Assert.AreEqual(2, r);
        }

        [TestMethod]
        public void TryParseTests()
        {
            int? left, right;
            
            // Testing invalid patterns
            Assert.IsFalse(RangeNumberMatcher.TryParse("..", out left, out right));
            Assert.IsFalse(RangeNumberMatcher.TryParse("*..", out left, out right));
            Assert.IsFalse(RangeNumberMatcher.TryParse("*..*", out left, out right));
            Assert.IsFalse(RangeNumberMatcher.TryParse("..*", out left, out right));
            
            // Testing valid patterns
            Assert.IsTrue(RangeNumberMatcher.TryParse("*", out left, out right));
            Assert.IsNull(left);
            Assert.IsNull(right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..", out left, out right));
            Assert.AreEqual(1, left);
            Assert.IsNull(right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("..2", out left, out right));
            Assert.IsNull(left);
            Assert.AreEqual(2, right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..2", out left, out right));
            Assert.AreEqual(1, left);
            Assert.AreEqual(2, right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("4", out left, out right));
            Assert.AreEqual(4, left);
            Assert.AreEqual(4, right);
            
            Assert.IsTrue(RangeNumberMatcher.TryParse("4..4", out left, out right));
            Assert.AreEqual(4, left);
            Assert.AreEqual(4, right);
            
            // Testing object factory
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..4", out var rnm));
            Assert.IsNotNull(rnm);
            Assert.AreEqual(1, rnm.Left);
            Assert.AreEqual(4, rnm.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("*", out rnm));
            Assert.IsNotNull(rnm);
            Assert.IsNull(rnm.Left);
            Assert.IsNull(rnm.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..", out rnm));
            Assert.IsNotNull(rnm);
            Assert.AreEqual(1, rnm.Left);
            Assert.IsNull(rnm.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("..4", out rnm));
            Assert.IsNotNull(rnm);
            Assert.IsNull(rnm.Left);
            Assert.AreEqual(4, rnm.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("1..1", out rnm));
            Assert.IsNotNull(rnm);
            Assert.AreEqual(1, rnm.Left);
            Assert.AreEqual(1, rnm.Right);
            Assert.IsTrue(RangeNumberMatcher.TryParse("1", out rnm));
            Assert.IsNotNull(rnm);
            Assert.AreEqual(1, rnm.Left);
            Assert.AreEqual(1, rnm.Right);
        }
    }
}