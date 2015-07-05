using System;
using System.Collections.Generic;
using System.Linq;
using NLog.Mustache.Extensions;
using Nustache.Core;

namespace NLog.Mustache
{
    public static class NustacheHelpers
    {
        public static void Register()
        {
            Helpers.Register("property", PropertyHelper);
            Helpers.Register("format", FormatHelper);
            Helpers.Register("replace", ReplaceHelper);
        }

        public static void PropertyHelper(
            RenderContext ctx,
            IList<object> args,
            IDictionary<string, object> options,
            RenderBlock fn,
            RenderBlock inverse)
        {
            try
            {
                ctx.Write(GetPropertyValue(args.FirstArg(), 
                    args.SecondStringArg(), args.ThirdStringArg()));
            }
            catch (Exception exception)
            {
                ctx.Write(exception.ToString());
            }
        }

        public static string GetPropertyValue(object source, 
            string propertyName, string format)
        {
            if (source == null || propertyName.IsNullOrEmpty()) return "";
            if (source is ExceptionModel)
            {
                var property = ((ExceptionModel)source).Properties
                    .FirstOrDefault(x => x.Name.Equals(propertyName,
                        StringComparison.OrdinalIgnoreCase));
                if (property != null) return property.Value.Format(format?.UrlDecode());
            }
            else
            {
                var property = source.GetType().GetProperties()
                    .FirstOrDefault(x => x.Name.Equals(propertyName,
                        StringComparison.OrdinalIgnoreCase));
                if (property != null) return property.GetValue(source)
                        .Format(format?.UrlDecode());
            }
            return "";
        }

        public static void FormatHelper(
            RenderContext ctx,
            IList<object> args,
            IDictionary<string, object> options,
            RenderBlock fn,
            RenderBlock inverse)
        {
            try
            {
                ctx.Write(FormatValue(args.FirstArg(), args.SecondStringArg()));
            }
            catch (Exception exception)
            {
                ctx.Write(exception.ToString());
            }
        }

        public static string FormatValue(object value, string format)
        {
            if (value == null) return "";
            if (format.IsNullOrEmpty()) return value.ToString();
            return value.Format(format.UrlDecode());
        }

        public static void ReplaceHelper(
            RenderContext ctx,
            IList<object> args,
            IDictionary<string, object> options,
            RenderBlock fn,
            RenderBlock inverse)
        {
            try
            {
                ctx.Write(ReplaceValue(args.FirstStringArg(), 
                    args.SecondStringArg(), args.ThirdStringArg()));
            }
            catch (Exception exception)
            {
                ctx.Write(exception.ToString());
            }
        }

        public static string ReplaceValue(string value, string find, string replace)
        {
            if (value.IsNullOrEmpty()) return "";
            if (find.IsNullOrEmpty() || replace.IsNullOrEmpty()) return value;
            return value.Replace(find.UrlDecode(), replace.UrlDecode());
        }
    }
}
