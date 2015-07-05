using System;
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
            public DateTime Oh { get; set; }
            public string Hai { get; set; }
            public decimal There { get; set; }
        }

        [Test]
        public void should_get_property_value()
        {
            NustacheHelpers.GetPropertyValue(new SomeClass { Hai = "yada"}, "Hai", null)
                .ShouldEqual("yada");
        }

        [Test]
        public void should_get_formatted_property_value()
        {
            NustacheHelpers.GetPropertyValue(
                new SomeClass { Oh = new DateTime(1985, 10, 26) }, "Oh", "yyyyMMdd")
                .ShouldEqual("19851026");
        }

        [Test]
        public void should_get_url_encoded_formatted_property_value()
        {
            NustacheHelpers.GetPropertyValue(
                new SomeClass { Oh = new DateTime(1985, 10, 26) }, "Oh", "yyyy%20MM%20dd")
                .ShouldEqual("1985 10 26");
        }

        [Test]
        public void should_get_formatted_property_value_with_curley_braces()
        {
            NustacheHelpers.GetPropertyValue(
                new SomeClass
                {
                    Oh = new DateTime(1985, 10, 26)
                }, "Oh", "}{yyyyMMdd")
                .ShouldEqual("}{19851026");
        }

        [Test]
        public void should_return_empty_string_when_property_formatting_fails()
        {
            NustacheHelpers.GetPropertyValue(
                new SomeClass { There = 169.32m }, "There", "Q2")
                .ShouldEqual("");
        }

        [Test]
        public void should_return_error_when_property_formatting_fails_and_debug_is_enabled()
        {
            NustacheHelpers.GetPropertyValue(
                new SomeClass { There = 169.32m }, "There", "!Q2")
                .ShouldEqual("Format specifier was invalid.");
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
                new ExceptionModel(new MyException("yada"), 0), "id", null)
                .ShouldEqual("123456");
        }

        public object[][] InvalidPropertyArgs =
        {
            new object[] { null, null, null },
            new object[] { new SomeClass(), null, null },
            new object[] { new SomeClass(), "yada", null },
            new object[] { new MyException("hai"), "yada", null }
        };

        [Test]
        [TestCaseSource(nameof(InvalidPropertyArgs))]
        public void should_return_empty_if_invalid_args(object source, 
            string propertyName, string format)
        {
            NustacheHelpers.GetPropertyValue(source, propertyName, format).ShouldEqual("");
        }

        [Test]
        public void should_get_formatted_value()
        {
            NustacheHelpers.FormatValue(new DateTime(1985, 10, 26), "yyyyMMdd")
                .ShouldEqual("19851026");
        }

        [Test]
        public void should_get_url_encoded_formatted_value()
        {
            NustacheHelpers.FormatValue(new DateTime(1985, 10, 26), "yyyy%20MM%20dd")
                .ShouldEqual("1985 10 26");
        }

        [Test]
        public void should_get_formatted_value_with_curley_braces()
        {
            NustacheHelpers.FormatValue(new DateTime(1985, 10, 26), "}{yyyyMMdd")
                .ShouldEqual("}{19851026");
        }

        [Test]
        public void should_return_empty_string_when_value_formatting_fails()
        {
            NustacheHelpers.FormatValue( 169.32m, "Q2")
                .ShouldEqual("");
        }

        [Test]
        public void should_return_error_when_value_formatting_fails_and_debug_is_enabled()
        {
            NustacheHelpers.FormatValue(169.32m, "!Q2")
                .ShouldEqual("Format specifier was invalid.");
        }

        [Test]
        public void should_replace_values()
        {
            NustacheHelpers.ReplaceValue("oh yo", "yo", "hai")
                .ShouldEqual("oh hai");
        }

        [Test]
        public void should_replace_url_encoded_value()
        {
            NustacheHelpers.ReplaceValue("oh yo", "%20yo", "%20hai")
                .ShouldEqual("oh hai");
        }
    }
}
