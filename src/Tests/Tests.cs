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

        [SetUp]
        public void Setup()
        {
            LogManager.ThrowExceptions = true;
            var loggingConfig = new LoggingConfiguration();

            _target = new MemoryTarget
            {
                Layout = @"${mustache:Template.html}"
            };
            loggingConfig.AddTarget("memory", _target);
            loggingConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, _target));

            LogManager.Configuration = loggingConfig;

            _logger = LogManager.GetLogger("Tests");
        }

        [Test]
        public void should()
        {
            _logger.Info("hai");
            _target.Logs.Count.ShouldEqual(1);
            _target.Logs.First().ShouldEqual("<b>Oh hai! Info</b>");
        }
    }
}
