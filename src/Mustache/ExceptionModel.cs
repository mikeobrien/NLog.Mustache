using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog.Mustache.Extensions;

namespace NLog.Mustache
{
    public class ExceptionModel
    {
        private readonly Exception _exception;

        public ExceptionModel(Exception exception, int number)
        {
            Number = number;
            _exception = exception;
            Properties = exception?.GetType().GetProperties()
                .Select(x => new ExceptionProperty(x.Name, 
                    x.GetValue(exception)))
                .Where(x => x.Name != "Data")
                .OrderBy(x => x.Name).ToList() ?? 
                new List<ExceptionProperty>();
        }

        public int Number { get; }
        public IDictionary Data => _exception?.Data;
        public string HelpLink => _exception?.HelpLink;
        public int HResult => _exception.WhenNotNull(x => x.HResult);
        public Exception InnerException => _exception?.InnerException;
        public string Message => _exception?.Message;
        public string Source => _exception?.Source;
        public string StackTrace => _exception?.StackTrace;
        public MethodBase TargetSite => _exception?.TargetSite;
        public List<ExceptionProperty> Properties { get; }

        public class ExceptionProperty
        {
            public ExceptionProperty(string name, object value)
            {
                Name = name;
                Value = value;
                HasValue = value != null && value as string != "";
            }

            public bool HasValue { get; }
            public string Name { get; set; }
            public object Value { get; set; }
        }
    }
}