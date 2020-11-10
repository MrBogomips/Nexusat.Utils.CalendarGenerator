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
        public void TryParseTest()
        {
            // Valid strings are:
            //   "" is a day info without a description and a working time
            //   "day description" is a day info with a description and without a working time
            //   "day description" 00:00-01:00,22:00-23:00 is a day info with a description and a working time
            //   "" 00:00-01:00 is a day info without a description and a working time
            //

            Assert.IsTrue(DayInfo.TryParse(@"[[]]", out var dayInfo));
            Assert.IsNull(dayInfo.Description);
            Assert.IsNull(dayInfo.WorkingPeriods);
            
            Assert.IsTrue(DayInfo.TryParse(@"[[Hello world]]", out dayInfo));
            Assert.AreEqual("Hello world",dayInfo.Description);
            Assert.IsNull(dayInfo.WorkingPeriods);
            
            Assert.IsTrue(DayInfo.TryParse(@"[[Hello world]] 08:00-12:00", out dayInfo));
            Assert.AreEqual("Hello world",dayInfo.Description);
            Assert.IsNotNull(dayInfo.WorkingPeriods);
            Assert.AreEqual(1, dayInfo.WorkingPeriods.Count());
            Assert.AreEqual("08:00-12:00", dayInfo.WorkingPeriods.First().ToString() );
            
        }

        [TestMethod]
        public void OverlapTimePeriodsTest()
        {
            TimePeriod.TryParseMulti("00:00-02:00 01:00-03:00", " ",out var tps);

            Assert.ThrowsException<ArgumentException>(() => new DayInfo(workingPeriods:tps));
        }
    }
}