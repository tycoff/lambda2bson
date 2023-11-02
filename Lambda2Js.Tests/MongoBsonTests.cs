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
        public class MyCustomClass
        {
            public string Title;

            public string Name { get; set; }

            public ChildElement Child { get; set; }
        }

        public class ChildElement
        {
            [BsonElement("child")]
            public ChildElement Child { get; set; }

            [BsonElement("testMongoSerializer")]
            public string Custom2 { get; set; }

            public IDictionary<string, string> Map { get; set; }
        }

        [TestMethod]
        public void TestingReferenceProperty()
        {
            Expression<Func<MyCustomClass, object>> expr = x => x.Name;

            var js = expr.CompileToBson();

            Assert.AreEqual("Name", js);
        }

        [TestMethod]
        public void TestingReferenceField()
        {
            Expression<Func<MyCustomClass, object>> expr = x => x.Title;

            var js = expr.CompileToBson();

            Assert.AreEqual("Title", js);
        }

        [TestMethod]
        public void TestingReferenceRecursiveProperty()
        {
            Expression<Func<MyCustomClass, object>> expr = x => x.Child.Child.Child.Child.Custom2;

            var js = expr.CompileToBson();

            Assert.AreEqual("Child.child.child.child.testMongoSerializer", js);
        }

        [TestMethod]
        public void TestingReferenceDictionaryWithConstantKey()
        {
            Expression<Func<MyCustomClass, object>> expr = x => x.Child.Map["Example1"];

            var js = expr.CompileToBson();

            Assert.AreEqual("Child.Map.Example1", js);
        }

        [TestMethod]
        public void TestingReferenceDictionaryWithVariableKey()
        {
            var key = "Example1";
            Expression<Func<MyCustomClass, object>> expr = x => x.Child.Map[key];

            var js = expr.CompileToBson();

            Assert.AreEqual("Child.Map.Example1", js);
        }

    }
}