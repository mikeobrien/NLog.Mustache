using System;
using System.Collections.Generic;
using NLog.Mustache;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class NustacheHelpersTests
    {
        public class SomeClass
        {
            public string Oh { get; set; }
            public string Hai { get; set; }
        }

        [Test]
        public void should_get_property_value()
        {
            NustacheHelpers.GetPropertyValue(
                new List<object> { new SomeClass { Hai = "yada"}, "Hai" })
                .ShouldEqual("yada");
        }

        public class MyException : Exception
        {
            public MyException(string message) : base(message) { }
            public string Id { get; } = "123456";
        }

        [Test]
        public void should_get_exception_property_value()
        {
            NustacheHelpers.GetPropertyValue(
                new List<object> { new ExceptionModel(new MyException("yada"), 0), "id" })
                .ShouldEqual("123456");
        }

        public object[][] InvalidPropertyArgs =
        {
            new object[] { null },
            new object[] { new List<object> { } },
            new object[] { new List<object> { null } },
            new object[] { new List<object> { new SomeClass() } },
            new object[] { new List<object> { null, null} },
            new object[] { new List<object> { new SomeClass(), "yada"} },
            new object[] { new List<object> { new MyException("hai"), "yada"} }
        };

        [Test]
        [TestCaseSource(nameof(InvalidPropertyArgs))]
        public void should_return_empty_if_invalid_args(List<object> args)
        {
            NustacheHelpers.GetPropertyValue(args).ShouldEqual("");
        }
    }
}
