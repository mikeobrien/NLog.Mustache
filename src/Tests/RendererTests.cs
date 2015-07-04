using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class RendererTests
    {
        private MemoryTarget _target;
        private Logger _logger;

        public void Setup(string templateName, bool debug)
        {
            LogManager.ThrowExceptions = true;
            var loggingConfig = new LoggingConfiguration();

            _target = new MemoryTarget
            {
                Layout = $"${{mustache:{templateName}:debug={debug}}}"
            };
            loggingConfig.AddTarget("memory", _target);
            loggingConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, _target));

            LogManager.Configuration = loggingConfig;

            _logger = LogManager.GetLogger("Tests");
        }

        [Test]
        public void should_render_template()
        {
            Setup("Template.html", false);
            _logger.Info("hai");
            _target.Logs.Count.ShouldEqual(1);
            _target.Logs.First().ShouldEqual("<b>Oh hai! Info</b>");
        }

        public class MyException : Exception
        {
            public MyException(string message, Exception exception) : base(message, exception) { }
            public string Id { get; } = "123456";
        }

        [Test]
        public void should_render_template_with_exception()
        {
            Setup("ExceptionTemplate.html", false);
            try
            {
                try
                {
                    var result = new Dictionary<string, string>()["yada"];
                }
                catch (Exception exception)
                {
                    throw new MyException("Oh noes!", exception);
                }
            }
            catch (Exception exception)
            {
                _logger.Fatal(exception, exception.Message);
            }
            _target.Logs.Count.ShouldEqual(1);
            var entry = _target.Logs.First();
            entry.ShouldContain("1: Oh noes!");
            entry.ShouldContain("Message: Oh noes!");
            entry.ShouldContain("2: The given key was not present in the dictionary.");
            entry.ShouldContain("Message: The given key was not present in the dictionary.");
        }

        [Test]
        public void should_render_template_with_property_helper()
        {
            Setup("PropertyHelperTemplate.html", false);
            _logger.Fatal(new MyException("hai", new Exception()), "yada");
            _target.Logs.Count.ShouldEqual(1);
            var entry = _target.Logs.First();
            entry.ShouldContain("property:123456:");
            entry.ShouldContain("current:123456:");
            entry.ShouldContain("sourcenotfound::");
            entry.ShouldContain("propnotfound::");
            entry.ShouldContain("object:Tests:");
        }

        [Test]
        public void should_render_empty_debug_template()
        {
            Setup("EmptyTemplate.html", true);
            _logger.Info("hai");
            _target.Logs.Count.ShouldEqual(1);
            _target.Logs.First().ShouldEqual("Embedded resource Tests.Templates.EmptyTemplate.html exists but is empty.");
        }

        [Test]
        public void should_render_missing_debug_template()
        {
            Setup("MissingTemplate.html", true);
            _logger.Info("hai");
            _target.Logs.Count.ShouldEqual(1);
            var entry = _target.Logs.First();
            entry.ShouldContain("Tests, ");
            entry.ShouldContain("Tests.Templates.Template.html");
        }
    }
}
