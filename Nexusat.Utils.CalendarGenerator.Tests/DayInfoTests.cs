using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class DayInfoTests
    {
        [TestMethod]
        public void DefaultCtorTest()
        {
            var di = new DayInfo();

            Assert.IsFalse(di.IsWorkingDay);
            Assert.IsNull(di.Description);
            Assert.IsNull(di.WorkingPeriods);
        }

        [TestMethod]
        public void OnlyDescriptionTest()
        {
            var desc = "description";
            var di = new DayInfo(desc);

            Assert.IsFalse(di.IsWorkingDay);
            Assert.AreEqual(desc, di.Description);
            Assert.IsNull(di.WorkingPeriods);
        }

        [TestMethod]
        public void EmptyWorkingPeriodsTest()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var wps = new List<TimePeriod>();
            var di = new DayInfo(workingPeriods: wps);

            Assert.IsFalse(di.IsWorkingDay);
            Assert.IsNull(di.Description);
            Assert.IsNull(di.WorkingPeriods);
        }

        [TestMethod]
        public void NonEmptyWorkingPeriodsTest()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var wps = new List<TimePeriod>
            {
                new TimePeriod()
            };
            var di = new DayInfo(workingPeriods: wps);

            Assert.IsTrue(di.IsWorkingDay);
            Assert.IsNull(di.Description);
            Assert.IsNotNull(di.WorkingPeriods);
        }

        [TestMethod]
        public void OverlapTimePeriodsTest()
        {
            var tps = TimePeriod.ParseMulti("00:00-02:00 01:00-03:00");

            Assert.ThrowsException<ArgumentException>(() => new DayInfo(workingPeriods:tps));
        }
    }
}