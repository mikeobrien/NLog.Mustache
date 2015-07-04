using System;
using System.Globalization;
using NLog;
using NLog.Mustache;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class MustacheTests
    {
        [Test]
        public void should_render_template()
        {
            var logEventInfo = new LogEventInfo(
                LogLevel.Fatal,
                "TestLogger",
                new CultureInfo("en-US"),
                "Error",
                new object[] { },
                new Exception("bad", new Exception("things")));

            Mustache.RenderLogEventInfo(logEventInfo, 
                "{{#exceptions}}{{number}}{{/exceptions}}", true)
                .ShouldEqual("12");
        }
    }
}
