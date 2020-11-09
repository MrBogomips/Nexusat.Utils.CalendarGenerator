using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nexusat.Utils.CalendarGenerator.CronDayRule;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class ModuloYearTests
    {
        [TestMethod]
        public void CtorTests()
        {
            var mym = new ModuloYearMatcher(null, null, 2);
            Assert.AreEqual(2, mym.Modulo);

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

            var mym = new ModuloYearMatcher(null, null, 3);
            Assert.IsFalse(mym.Match(dt2000));
            Assert.IsTrue(mym.Match(dt2001));
            Assert.IsTrue(mym.Match(dt3000));
            Assert.IsTrue(mym.Match(dt3003));
            
            mym = new ModuloYearMatcher(2500, null, 3);
            Assert.IsFalse(mym.Match(dt2000));
            Assert.IsFalse(mym.Match(dt2001));
            Assert.IsTrue(mym.Match(dt3000));
            Assert.IsTrue(mym.Match(dt3003));
            
            mym = new ModuloYearMatcher(null, 3000, 3);
            Assert.IsFalse(mym.Match(dt2000));
            Assert.IsTrue(mym.Match(dt2001));
            Assert.IsTrue(mym.Match(dt3000));
            Assert.IsFalse(mym.Match(dt3003));
        }
    }
}