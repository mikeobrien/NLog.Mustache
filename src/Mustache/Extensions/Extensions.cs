using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace NLog.Mustache.Extensions
{
    public static class AssemblyExtensions
    {
        public static TValue AddValue<TKey, TValue>(
            this ConcurrentDictionary<TKey, TValue> dictionary, 
            TKey key,
            TValue value)
        {
            dictionary.TryAdd(key, value);
            return value;
        }

        public static TResult WhenNotNullOrEmpty<TResult>(this string source, Func<string, TResult> value)
        {
            return string.IsNullOrEmpty(source) ? default(TResult) : value(source);
        }

        public static TResult WhenNotNull<T, TResult>(this T source, Func<T, TResult> value)
        {
            return source == null ? default(TResult) : value(source);
        }

        public static string FindResourceString(this string resourceName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetManifestResourceNames()
                    .Where(n => n.ToLower().EndsWith(resourceName.ToLower()))
                    .Select(n => new { Name = n, Assembly = a }))
                .OrderBy(x => x.Name.Length)
                .FirstOrDefault()
                .WhenNotNull(x => new StreamReader(x.Assembly.GetManifestResourceStream(x.Name)))
                .WhenNotNull(x => x.ReadToEnd());
        }
    }
}