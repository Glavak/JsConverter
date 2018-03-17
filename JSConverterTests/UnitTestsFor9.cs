using System;
using JSConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JSConverterTests
{
    [TestClass]
    public class UnitTestsFor9
    {
        [TestMethod]
        public void TestClosure()
        {
            var yearFrom = DateTime.Today.Year - 18;
            string result = Js<User>.Convert(u => DateTime.Today.Year - u.Age > yearFrom);

            Assert.AreEqual("function (u) { return ((2018 - u.Age) > 2000); }", result);
        }
    }
}
