using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Repower.Calendar.Test
{
    [TestClass]
    public class TimeTests
    {
        [TestMethod]
        public void OperatorsTest()
        {
            Time t1 = new Time(0, 0);
            Time t2 = new Time(0, 1);
            Time t3 = new Time(1, 0);
            Time t4 = new Time(0, 0);

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
    }
}
