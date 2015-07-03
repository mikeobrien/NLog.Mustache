using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
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

        public bool Debug { get; set; }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var template = TemplateCache.ContainsKey(Template) ? TemplateCache[Template] : 
                TemplateCache.AddValue(Template, FindTemplate(Template, Debug));

            try
            {
                builder.Append(Nustache.Core.Render.StringToString(template, logEvent));
            }
            catch (Exception exception)
            {
                if (Debug) builder.Append($"Error rendering template: {exception}");
            }
        }

        public static string FindTemplate(string resourceName, bool debug)
        {
            var assemblies = AppDomain.CurrentDomain.GetNonClrAssemblies();
            var resource = assemblies
                .SelectMany(a => a.GetManifestResourceNames()
                    .Where(n => n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase))
                    .Select(n => new { Name = n, Assembly = a }))
                .OrderBy(x => x.Name.Length)
                .FirstOrDefault();
            var value = resource.WhenNotNull(x => x.Assembly.GetResourceString(x.Name));

            if (!debug || !string.IsNullOrEmpty(value)) return value;
            if (resource != null) return $"Embedded resource {resource.Name} exists but is empty.";
            var resources = assemblies
                .Select(x => new { Assembly = x, Resources = x.GetManifestResourceNames() })
                .Where(x => x.Resources.Any())
                .Select(x => $"{x.Assembly.FullName}:\r\n\t{x.Resources.Join("\r\n\t")}").Join("\r\n");
            return $"Embedded resource {resourceName} not found. The folowing resources were found:\r\n{resources}";
        }
    }
}
