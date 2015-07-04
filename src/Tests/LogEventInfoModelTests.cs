using System;
using System.Globalization;
using NLog;
using NLog.Mustache;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class LogEventInfoModelTests
    {
        [Test]
        public void should_map_log_event_info_and_exceptions()
        {
            var logEventInfo = new LogEventInfo(
                LogLevel.Fatal, 
                "TestLogger", 
                new CultureInfo("en-US"), 
                "Error", 
                new object[] {}, 
                new Exception("bad", new Exception("things")));
            var model = new LogEventInfoModel(logEventInfo);

            model.SequenceID.ShouldEqual(logEventInfo.SequenceID);
            model.TimeStamp.ShouldEqual(logEventInfo.TimeStamp);
            model.Level.ShouldEqual(logEventInfo.Level);
            model.HasStackTrace.ShouldEqual(logEventInfo.HasStackTrace);
            model.UserStackFrame.ShouldEqual(logEventInfo.UserStackFrame);
            model.UserStackFrameNumber.ShouldEqual(logEventInfo.UserStackFrameNumber);
            model.StackTrace.ShouldEqual(logEventInfo.StackTrace);
            model.LoggerName.ShouldEqual(logEventInfo.LoggerName);
            model.LoggerShortName.ShouldEqual(logEventInfo.LoggerShortName);
            model.Message.ShouldEqual(logEventInfo.Message);
            model.Parameters.ShouldEqual(logEventInfo.Parameters);
            model.FormatProvider.ShouldEqual(logEventInfo.FormatProvider);
            model.FormattedMessage.ShouldEqual(logEventInfo.FormattedMessage);
            model.Properties.ShouldEqual(logEventInfo.Properties);

            model.Exception.Message.ShouldEqual(logEventInfo.Exception.Message);
            model.Exceptions.Count.ShouldEqual(2);
            model.Exceptions[0].Number.ShouldEqual(1);
            model.Exceptions[0].Message.ShouldEqual(logEventInfo.Exception.Message);
            model.Exceptions[1].Number.ShouldEqual(2);
            model.Exceptions[1].Message.ShouldEqual(logEventInfo.Exception.InnerException.Message);
        }

        [Test]
        public void should_not_fail_if_no_exception()
        {
            var logEventInfo = new LogEventInfo();
            var model = new LogEventInfoModel(logEventInfo);

            model.Exception.ShouldBeNull();
            model.Exceptions.ShouldBeEmpty();
        }
    }
}
