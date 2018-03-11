using JSConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSConverterTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSimpleOperators()
        {
            string result = Js<int>.Convert(x => x + 1 * x);

            Assert.AreEqual("function (x) { return (x + (1 * x)); }", result);
        }

        [TestMethod]
        public void TestConditionalOperator()
        {
            string result = Js<int>.Convert(x => x > 2 ? 3 : 1);

            Assert.AreEqual("function (x) { return ((x > 2) ? 3 : 1); }", result);
        }

        [TestMethod]
        public void TestFuncName()
        {
            string result = Js<int>.Convert(x => x, "SomeName");

            Assert.AreEqual("function SomeName(x) { return x; }", result);
        }
    }
}
