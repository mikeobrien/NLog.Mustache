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
                ctx.Write(GetPropertyValue(args));
            }
            catch (Exception exception)
            {
                ctx.Write(exception.ToString());
            }
        }

        public static string GetPropertyValue(IList<object> args)
        {
            if (args == null || args.Count < 2) return "";
            var source = args[0];
            var propertyName = args[1] as string;
            var format = args.Count > 2 ? args[2] as string : null;
            if (source == null || string.IsNullOrEmpty(propertyName)) return "";
            if (source is ExceptionModel)
            {
                var property = ((ExceptionModel)source).Properties
                    .FirstOrDefault(x => x.Name.Equals(propertyName,
                        StringComparison.OrdinalIgnoreCase));
                if (property != null) return property.Value.Format(format);
            }
            else
            {
                var property = source.GetType().GetProperties()
                    .FirstOrDefault(x => x.Name.Equals(propertyName,
                        StringComparison.OrdinalIgnoreCase));
                if (property != null) return property.GetValue(source).Format(format);
            }
            return "";
        }
    }
}
