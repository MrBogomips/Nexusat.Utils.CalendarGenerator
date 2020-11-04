using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Repower.Calendar.Test
{
    [TestClass]
    public class TimeTests
    {
        static readonly Time t1 = new Time(0, 0);
        static readonly Time t2 = new Time(0, 1);
        static readonly Time t3 = new Time(1, 0);
        static readonly Time t4 = new Time(0, 0);

        [TestMethod]
        public void CtorTest()
        {
            new Time(24, 0);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Time(-1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Time(25, 0));

            new Time(0, 59);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Time(0, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Time(0, 60));
        }

        [TestMethod]
        public void OperatorsTest()
        {
            Assert.IsTrue(t1 == t4);
            Assert.IsTrue(t1 >= t4);
            Assert.IsTrue(t1 <= t4);

            Assert.IsTrue(t1 != t2);
            Assert.IsTrue(t1 != t3);

            Assert.IsTrue(t1 < t2);
            Assert.IsTrue(t1 <= t2);
            Assert.IsTrue(t2 > t1);
            Assert.IsTrue(t2 >= t1);
        }

        [TestMethod]
        public void ComparableTest()
        {
            Assert.IsTrue(t1.CompareTo(t2) < 0);
            Assert.IsTrue(t2.CompareTo(t1) > 0);
            Assert.IsTrue(t1.CompareTo(t4) == 0);
        }
        [TestMethod]
        public void EqualsTest()
        {
            object o1 = t1;
            object o2 = t4;

            Assert.AreEqual(o1, o2);
        }
        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("00:00", t1.ToString());
            Assert.AreEqual("00:01", t2.ToString());
            Assert.AreEqual("01:00", t3.ToString());
        }
        [TestMethod]
        public void GetSerialTest()
        {
            var t0 = new Time();
            var t1 = new Time(0, 1);
            var t2 = new Time(1, 0);
            var t3 = new Time(1, 1);
            var t4 = new Time(23, 59);
            var t5 = new Time(24, 0);

            Assert.AreEqual(0, t0.GetSerial());
            Assert.AreEqual(1, t1.GetSerial());
            Assert.AreEqual(60, t2.GetSerial());
            Assert.AreEqual(61, t3.GetSerial());
            Assert.AreEqual(24 * 60 - 1, t4.GetSerial());
            Assert.AreEqual(24 * 60, t5.GetSerial());
        }
    }
}
