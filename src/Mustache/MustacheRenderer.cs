using System.Collections.Concurrent;
using System.Text;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Mustache.Extensions;

namespace NLog.Mustache
{
    [LayoutRenderer("mustache")]
    public class MustachRenderer : LayoutRenderer
    {
        private static readonly ConcurrentDictionary<string, string> TemplateCache = 
            new ConcurrentDictionary<string, string>();

        [DefaultParameter, RequiredParameter]
        public string Template { get; set; }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var template = TemplateCache.ContainsKey(Template) ? TemplateCache[Template] : 
                TemplateCache.AddValue(Template, Template.FindResourceString());
            
            builder.Append(Nustache.Core.Render.StringToString(template, logEvent));
        }
    }
}
