using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class Tests
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

        [Test]
        public void should_render_empty_debug_template()
        {
            Setup("EmptyTemplate.html", true);
            _logger.Info("hai");
            _target.Logs.Count.ShouldEqual(1);
            _target.Logs.First().ShouldEqual("Embedded resource Tests.EmptyTemplate.html exists but is empty.");
        }

        [Test]
        public void should_render_missing_debug_template()
        {
            Setup("MissingTemplate.html", true);
            _logger.Info("hai");
            _target.Logs.Count.ShouldEqual(1);
            var entry = _target.Logs.First();
            entry.ShouldContain("Tests, ");
            entry.ShouldContain("Tests.Template.html");
        }
    }
}
