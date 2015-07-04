using System;
using System.Text;
using NLog.Config;
using NLog.LayoutRenderers;

namespace NLog.Mustache
{
    [LayoutRenderer("mustache")]
    public class Renderer : LayoutRenderer
    {
        static Renderer()
        {
            NustacheHelpers.Register();
        }

        [DefaultParameter, RequiredParameter]
        public string TemplateName { get; set; }

        public bool Debug { get; set; }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            try
            {
                builder.Append(Mustache.RenderLogEventInfo(logEvent, Template.Load(TemplateName, Debug), Debug));
            }
            catch (Exception exception)
            {
                if (Debug) builder.Append($"Error rendering template: {exception}");
            }
        }
    }
}
