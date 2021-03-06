﻿using System;
using System.Collections.Generic;
using NLog.Mustache;
using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    public class ExceptionModelTests
    {
        [Test]
        public void should_map_exception_and_enumerate_properties()
        {
            Exception exception = null;

            try
            {
                var result = new Dictionary<string, string>()["yada"];
            }
            catch (Exception innerException)
            {
                exception = new Exception("Oh noes!", innerException);
            }

            var model = new ExceptionModel(exception, 1);
            model.Number.ShouldEqual(1);
            model.Data.ShouldEqual(exception.Data);
            model.HelpLink.ShouldEqual(exception.HelpLink);
            model.HResult.ShouldEqual(exception.HResult);
            model.InnerException.ShouldEqual(exception.InnerException);
            model.Message.ShouldEqual(exception.Message);
            model.Source.ShouldEqual(exception.Source);
            model.StackTrace.ShouldEqual(exception.StackTrace);
            model.TargetSite.ShouldEqual(exception.TargetSite);
            model.Type.ShouldEqual(exception.GetType());

            model.Properties.Count.ShouldEqual(6);
            model.Properties[0].Name.ShouldEqual("Type");
            model.Properties[0].Value.ShouldEqual(exception.GetType());
            model.Properties[0].HasValue.ShouldBeTrue();
            model.Properties[1].Name.ShouldEqual("HelpLink");
            model.Properties[1].Value.ShouldEqual(exception.HelpLink);
            model.Properties[1].HasValue.ShouldBeFalse();
            model.Properties[2].Name.ShouldEqual("Message");
            model.Properties[2].Value.ShouldEqual(exception.Message);
            model.Properties[2].HasValue.ShouldBeTrue();
            model.Properties[3].Name.ShouldEqual("Source");
            model.Properties[3].Value.ShouldEqual(exception.Source);
            model.Properties[3].HasValue.ShouldBeFalse();
            model.Properties[4].Name.ShouldEqual("StackTrace");
            model.Properties[4].Value.ShouldEqual(exception.StackTrace);
            model.Properties[4].HasValue.ShouldBeFalse();
            model.Properties[5].Name.ShouldEqual("TargetSite");
            model.Properties[5].Value.ShouldEqual(exception.TargetSite);
            model.Properties[5].HasValue.ShouldBeFalse();
        }

        [Test]
        public void should_empty_if_exception_is_null()
        {
            var model = new ExceptionModel(null, 1);
            model.Number.ShouldEqual(1);
            model.Data.ShouldBeNull();
            model.HelpLink.ShouldBeNull();
            model.HResult.ShouldEqual(0);
            model.InnerException.ShouldBeNull();
            model.Message.ShouldBeNull();
            model.Source.ShouldBeNull();
            model.StackTrace.ShouldBeNull();
            model.TargetSite.ShouldBeNull();
            model.Properties.Count.ShouldEqual(0);
        }
    }
}
