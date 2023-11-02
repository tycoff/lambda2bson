using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Lambda2Js.Bson.Tests
{
    [TestClass]
    public class MongoBsonTests
    {
        public class MyTestClass
        {
            public string FieldName;

            public MyTestClass Child { get; set; }

            [BsonElement("_name")]
            public string Name { get; set; }

            public IDictionary<string, string> Map { get; set; }
        }

        [TestMethod]
        public void UsingPropertyWithAttribute()
        {
            Expression<Func<MyTestClass, object>> expr = x => x.Name;

            var js = expr.CompileToBson();

            Assert.AreEqual("_name", js);
        }

        [TestMethod]
        public void UsingProperty()
        {
            Expression<Func<MyTestClass, object>> expr = x => x.Child;

            var js = expr.CompileToBson();

            Assert.AreEqual("Child", js);
        }

        [TestMethod]
        public void UsingNestedProperty()
        {
            Expression<Func<MyTestClass, object>> expr = x => x.Child.Child.Child.Child;

            var js = expr.CompileToBson();

            Assert.AreEqual("Child.Child.Child.Child", js);
        }

        [TestMethod]
        public void UsingMapWithConstantValue()
        {
            Expression<Func<MyTestClass, object>> expr = x => x.Map["Example"];

            var js = expr.CompileToBson();

            Assert.AreEqual("Map.Example", js);
        }

        [TestMethod]
        public void UsingMapWithVaryingValue()
        {
            var key = "Example";
            Expression<Func<MyTestClass, object>> expr = x => x.Map[key];

            var js = expr.CompileToBson();

            Assert.AreEqual("Map.Example", js);
        }

        [TestMethod]
        public void UsingMapWithCoalesced()
        {
            string key = null;
            Expression<Func<MyTestClass, object>> expr = x => x.Map[key ?? "Example"];

            var js = expr.CompileToBson();

            Assert.AreEqual("Map.Example", js);
        }
    }
}