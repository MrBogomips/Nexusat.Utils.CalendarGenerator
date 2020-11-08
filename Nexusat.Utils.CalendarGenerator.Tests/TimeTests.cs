using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable HeapView.BoxingAllocation

namespace Nexusat.Utils.CalendarGenerator.Tests
{
    [TestClass]
    public class TimeTests
    {
        private static readonly Time T1 = new Time(0, 0);
        private static readonly Time T2 = new Time(0, 1);
        private static readonly Time T3 = new Time(1, 0);
        private static readonly Time T4 = new Time(0, 0);

        [TestMethod]
        public void CtorTest()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new Time(24, 0);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Time(-1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Time(25, 0));

            // ReSharper disable once ObjectCreationAsStatement
            new Time(0, 59);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Time(0, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Time(0, 60));
        }

        [TestMethod]
        public void OperatorsTest()
        {
            Assert.IsTrue(T1 == T4);
            Assert.IsTrue(T1 >= T4);
            Assert.IsTrue(T1 <= T4);

            Assert.IsTrue(T1 != T2);
            Assert.IsTrue(T1 != T3);

            Assert.IsTrue(T1 < T2);
            Assert.IsTrue(T1 <= T2);
            Assert.IsTrue(T2 > T1);
            Assert.IsTrue(T2 >= T1);
        }

        [TestMethod]
        public void ComparableTest()
        {
            Assert.IsTrue(T1.CompareTo(T2) < 0);
            Assert.IsTrue(T2.CompareTo(T1) > 0);
            Assert.IsTrue(T1.CompareTo(T4) == 0);
        }

        [TestMethod]
        public void EqualsTest()
        {
            object o1 = T1;
            object o2 = T4;

            Assert.AreEqual(o1, o2);
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("00:00", T1.ToString());
            Assert.AreEqual("00:01", T2.ToString());
            Assert.AreEqual("01:00", T3.ToString());
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

        [TestMethod]
        public void ParseTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Time.Parse(null));
            Assert.ThrowsException<ArgumentException>(() => Time.Parse("xxx"));
            var t = Time.Parse("00:00");
            Assert.AreEqual(0, t.Hour);
            Assert.AreEqual(0, t.Minute);
            t = Time.Parse("23:59");
            Assert.AreEqual(23, t.Hour);
            Assert.AreEqual(59, t.Minute);
            t = Time.Parse("24:00");
            Assert.AreEqual(24, t.Hour);
            Assert.AreEqual(00, t.Minute);
        }
    }
}