using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog.Mustache.Extensions;

namespace NLog.Mustache
{
    public class LogEventInfoModel
    {
        private readonly LogEventInfo _info;

        public LogEventInfoModel(LogEventInfo info)
        {
            _info = info;
            var number = 1;
            Exceptions = _info.Exception.Flatten(x => x.InnerException)
                .Select(x => new ExceptionModel(x, number++)).ToList();
            if (info.Exception != null) Exception = 
                new ExceptionModel(info.Exception, 1);
        }

        public int SequenceID => _info.SequenceID;
        public DateTime TimeStamp => _info.TimeStamp;
        public LogLevel Level => _info.Level;
        public bool HasStackTrace => _info.HasStackTrace;
        public StackFrame UserStackFrame => _info.UserStackFrame;
        public int UserStackFrameNumber => _info.UserStackFrameNumber;
        public StackTrace StackTrace => _info.StackTrace;
        public ExceptionModel Exception { get; }
        public List<ExceptionModel> Exceptions { get; }
        public string LoggerName => _info.LoggerName;
        public string LoggerShortName => _info.LoggerShortName;
        public string Message => _info.Message;
        public object[] Parameters => _info.Parameters;
        public IFormatProvider FormatProvider => _info.FormatProvider;
        public string FormattedMessage => _info.FormattedMessage;
        public IDictionary<object, object> Properties => _info.Properties;
    }
}
