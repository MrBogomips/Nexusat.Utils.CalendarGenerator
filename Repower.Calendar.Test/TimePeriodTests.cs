using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repower.Calendar.Test
{
    [TestClass]
    public class TimePeriodTests
    {
        [TestMethod]
        public void OverlapsTest()
        {
            /* Overlaps is a SYMMETRIC relation, therefore are tested both the directios */

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
            Assert.AreEqual(0, p0.GetMinutes());

            var p1 = new TimePeriod(new Time(), new Time(0, 1));
            Assert.AreEqual(1, p1.GetMinutes());

            var p2 = new TimePeriod(new Time(), new Time(1, 0));
            Assert.AreEqual(60, p2.GetMinutes());

            var p3 = new TimePeriod(new Time(), new Time(24, 0));
            Assert.AreEqual(60 * 24, p3.GetMinutes());
            var p4 = new TimePeriod(new Time(), new Time(23, 59));
            Assert.AreEqual(60 * 24 - 1, p4.GetMinutes());
        }
    }
}
