using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
