using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class TimePeriodTests
    {
        [TestMethod]
        public void OverlapsTest()
        {
            /* Overlaps is a SYMMETRIC relation, therefore are tested both the directions */

            /* t0    +---------------+
             * t1           +------------+
             * OVERLAPS */
            {
                var t0 = new TimePeriod(new Time(10, 0), new Time(12, 0));
                var t1 = new TimePeriod(new Time(11, 0), new Time(13, 0));
                Assert.IsTrue(t0.Overlaps(t1));
                Assert.IsTrue(t1.Overlaps(t0));
            }
            /* t0    +---------------+
             * t1         +-------+
             * OVERLAPS */
            {
                var t0 = new TimePeriod(new Time(10, 0), new Time(12, 0));
                var t1 = new TimePeriod(new Time(10, 30), new Time(11, 30));
                Assert.IsTrue(t0.Overlaps(t1));
                Assert.IsTrue(t1.Overlaps(t0));
            }
            /* t0    +----+
             * t1         +-------+
             * DONT OVERLAPS */
            {
                var t0 = new TimePeriod(new Time(10, 0), new Time(12, 0));
                var t1 = new TimePeriod(new Time(12, 0), new Time(12, 30));
                Assert.IsFalse(t0.Overlaps(t1));
                Assert.IsFalse(t1.Overlaps(t0));
            }
            /* t0    +----+
             * t1         +
             * DONT OVERLAPS */
            {
                var t0 = new TimePeriod(new Time(10, 0), new Time(12, 0));
                var t1 = new TimePeriod(new Time(12, 0), new Time(12, 0));
                Assert.IsFalse(t0.Overlaps(t1));
                Assert.IsFalse(t1.Overlaps(t0));
            }
        }

        [TestMethod]
        public void GetMinutesTest()
        {
            var p0 = new TimePeriod();
            Assert.AreEqual(0, p0.TotalMinutes);

            var p1 = new TimePeriod(new Time(), new Time(0, 1));
            Assert.AreEqual(1, p1.TotalMinutes);

            var p2 = new TimePeriod(new Time(), new Time(1, 0));
            Assert.AreEqual(60, p2.TotalMinutes);

            var p3 = new TimePeriod(new Time(), new Time(24, 0));
            Assert.AreEqual(60 * 24, p3.TotalMinutes);
            var p4 = new TimePeriod(new Time(), new Time(23, 59));
            Assert.AreEqual(60 * 24 - 1, p4.TotalMinutes);
        }

        [TestMethod]
        public void GetTimeSpanTest()
        {
            var p0 = new TimePeriod();
            Assert.AreEqual(0, ((TimeSpan) p0).TotalMinutes);

            var p1 = new TimePeriod(new Time(), new Time(0, 1));
            Assert.AreEqual(1, ((TimeSpan) p1).TotalMinutes);

            var p2 = new TimePeriod(new Time(), new Time(1, 0));
            Assert.AreEqual(60, ((TimeSpan) p2).TotalMinutes);

            var p3 = new TimePeriod(new Time(), new Time(24, 0));
            Assert.AreEqual(60 * 24, ((TimeSpan) p3).TotalMinutes);
            var p4 = new TimePeriod(new Time(), new Time(23, 59));
            Assert.AreEqual(60 * 24 - 1, ((TimeSpan) p4).TotalMinutes);
        }

        [TestMethod]
        public void ParseTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => TimePeriod.Parse(null));
            Assert.ThrowsException<ArgumentException>(() => TimePeriod.Parse("xxx"));
            var tp = TimePeriod.Parse("00:00-24:00");
            Assert.AreEqual(Time.Parse("00:00"), tp.Begin);
            Assert.AreEqual(Time.Parse("24:00"), tp.End);
        }
        
        [TestMethod]
        public void ParseMultiTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => TimePeriod.Parse(null));
            Assert.ThrowsException<ArgumentException>(() => TimePeriod.Parse("xxx"));
            Assert.ThrowsException<ArgumentException>(() => TimePeriod.Parse(""));
            var tps = TimePeriod.ParseMulti("00:00-24:00");
            Assert.AreEqual(1, tps.Count());
            tps = TimePeriod.ParseMulti("00:00-12:00 00:00-12:00");
            Assert.AreEqual(2, tps.Count());
        }
    }
}