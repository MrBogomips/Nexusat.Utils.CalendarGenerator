using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class ModuloNumberTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var mnm = new ModuloNumberMatcher(null, null, 2);
            Assert.AreEqual(2, mnm.Modulo);

            Assert.ThrowsException<ArgumentException>(() => new ModuloNumberMatcher(2000, 2000, 2),
                "Single year range invalid");
            Assert.ThrowsException<ArgumentException>(() => new ModuloNumberMatcher(2000, 3000, 1),
                "Invalid modulo");
        }

        [TestMethod]
        public void ToStringTests()
        {
            Assert.AreEqual("*%2",new ModuloNumberMatcher(null, null, 2).ToString());
            Assert.AreEqual("2000..%3",new ModuloNumberMatcher(2000, null, 3).ToString());
            Assert.AreEqual("..3000%7",new ModuloNumberMatcher(null, 3000, 7).ToString());
        }

        [TestMethod]
        public void MatchTests()
        {
            var n2000 = 2000;
            var n2001 = 2001;
            var n3000 = 3000;
            var n3003 = 3003;

            var mnm = new ModuloNumberMatcher(null, null, 3);
            Assert.IsFalse(mnm.Match(n2000));
            Assert.IsTrue(mnm.Match(n2001));
            Assert.IsTrue(mnm.Match(n3000));
            Assert.IsTrue(mnm.Match(n3003));
            
            mnm = new ModuloNumberMatcher(2500, null, 3);
            Assert.IsFalse(mnm.Match(n2000));
            Assert.IsFalse(mnm.Match(n2001));
            Assert.IsTrue(mnm.Match(n3000));
            Assert.IsTrue(mnm.Match(n3003));
            
            mnm = new ModuloNumberMatcher(null, 3000, 3);
            Assert.IsFalse(mnm.Match(n2000));
            Assert.IsTrue(mnm.Match(n2001));
            Assert.IsTrue(mnm.Match(n3000));
            Assert.IsFalse(mnm.Match(n3003));
        }
    }
}