using System;
using System.Collections.Concurrent;
using System.Linq;
using NLog.Mustache.Extensions;

namespace NLog.Mustache
{
    public static class Template
    {
        private static readonly ConcurrentDictionary<string, string> TemplateCache =
            new ConcurrentDictionary<string, string>();

        public static string Load(string templateName, bool debug)
        {
            return TemplateCache.ContainsKey(templateName) ? TemplateCache[templateName] :
                TemplateCache.AddValue(templateName, Find(templateName, debug));
        }

        private static string Find(string resourceName, bool debug)
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
            return $"Embedded resource {resourceName} not found. " +
                $"The folowing resources were found:\r\n{resources}";
        }
    }
}
