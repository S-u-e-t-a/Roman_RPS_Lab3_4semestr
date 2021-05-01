using Microsoft.VisualStudio.TestTools.UnitTesting;
using laba3;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            double x = 2;
            double a = 5;
            double expected = a * a * a / (a * a + x * x);
            double actual = Model.Function(x, a);
            Assert.AreEqual(expected,actual);
        }

        [TestMethod]
        public void TestMethod2()
        {
            double x = 0;
            double a = -23;
            double expected = a * a * a / (a * a + x * x);
            double actual = Model.Function(x, a);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod3()
        {
            double x = 6;
            double a = 45;
            double expected = a * a * a / (a * a + x * x);
            double actual = Model.Function(x, a);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod4()
        {
            double x = 36;
            double a = 0;
            double expected = a * a * a / (a * a + x * x);
            double actual = Model.Function(x, a);
            Assert.AreEqual(expected, actual);
        }
    }
}
