using System;
using System.Linq;
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

        [TestMethod]
        public void TestEnums()
        {
            string result = Js<User>.Convert(u => u.Gender == Gender.Female);

            Assert.AreEqual("function (u) { return (u.Gender == 1); }", result);
        }

        [TestMethod]
        public void TestLinqSelect()
        {
            string result = Js<User[]>.Convert((x) => x.Select(t => t.Name));

            Assert.AreEqual("function (x) { var r = []; for(var i=0;i<x.length;i++) { r.push(x[i].Name); } return r; }", result);
        }

        [TestMethod]
        public void TestLinqWhere()
        {
            string result = Js<User[], int>.Convert((x, age) => x.Where(t => t.Age > age));

            Assert.AreEqual("function (x, age) { var r = []; for(var i=0;i<x.length;i++) { if (x[i].Age > age) r.push(x[i]); } return r; }", result);
        }

        [TestMethod]
        public void TestLinqCombined()
        {
            string result = Js<User[], int>.Convert((x, age) => from t in x where t.Age > age select t.Name);

            Assert.AreEqual("function (x, age) { var r = []; for(var i=0;i<x.length;i++) { if (x[i].Age > age) r.push(x[i].Name); } return r; }", result);
        }
    }
}
