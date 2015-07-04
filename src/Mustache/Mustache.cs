using System;

namespace NLog.Mustache
{
    public static class Mustache
    {
        public static string RenderLogEventInfo(LogEventInfo logEvent, string template, bool debug)
        {
            try
            {
                return Nustache.Core.Render.StringToString(
                    template, new LogEventInfoModel(logEvent));
            }
            catch (Exception exception)
            {
                if (debug) return $"Error rendering template: {exception}";
            }
            return "";
        }
    }
}
