using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class PeriodicNumberTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var pnm = new PeriodicNumberMatcher(2000, null, 2);
            Assert.AreEqual(2, pnm.Period);

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

            var pnm = new PeriodicNumberMatcher(2000, null, 2);
            Assert.IsTrue(pnm.Match(n2000));
            Assert.IsFalse(pnm.Match(n2001));
            Assert.IsTrue(pnm.Match(n2002));
            Assert.IsFalse(pnm.Match(n2003));
            pnm = new PeriodicNumberMatcher(2001, null, 2);
            Assert.IsFalse(pnm.Match(n2000));
            Assert.IsTrue(pnm.Match(n2001));
            Assert.IsFalse(pnm.Match(n2002));
            Assert.IsTrue(pnm.Match(n2003));
            pnm = new PeriodicNumberMatcher(2002, null, 2);
            Assert.IsFalse(pnm.Match(n2000));
            Assert.IsFalse(pnm.Match(n2001));
            Assert.IsTrue(pnm.Match(n2002));
            Assert.IsFalse(pnm.Match(n2003));
        }
    }
}