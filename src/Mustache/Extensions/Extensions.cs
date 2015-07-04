﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NLog.Mustache.Extensions
{
    public static class AssemblyExtensions
    {
        public static TValue AddValue<TKey, TValue>(
            this ConcurrentDictionary<TKey, TValue> dictionary, 
            TKey key, TValue value)
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

        public static string StripFirst(this string value)
        {
            return value.WhenNotNull(x => string.IsNullOrEmpty(value) || 
                x.Length == 1 ? "" : x.Substring(1));
        }

        public static string Format(this object value, string format)
        {
            if (value == null) return "";
            if (string.IsNullOrEmpty(format)) return value.ToString();

            var debug = format.StartsWith("!");
            format = debug ? format.StripFirst() : format;
            format = format.Replace("{", "{{").Replace("}", "}}");

            try
            {
                return string.Format($"{{0:{format}}}", value);
            }
            catch (Exception exception)
            {
                return debug ? exception.Message : "";
            }
        }

        public static string Join(this IEnumerable<string> source, string seperator)
        {
            return source != null && source.Any() ? 
                source.Aggregate((a, i) => $"{a}{seperator}{i}") : "";
        }

        public static string GetResourceString(this Assembly assembly, string name)
        {
            return new StreamReader(assembly.GetManifestResourceStream(name))
                .WhenNotNull(x => x.ReadToEnd());
        }

        public static IList<Assembly> GetNonClrAssemblies(this AppDomain domain)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic &&
                    !x.FullName.StartsWith("mscorlib,") &&
                    !x.FullName.StartsWith("System,") &&
                    !x.FullName.StartsWith("System.")).ToList();
        }

        public static IEnumerable<T> Flatten<T>(this T source, Func<T, T> iterator) where T : class
        {
            var result = source;
            while (result != null)
            {
                yield return result;
                result = iterator(result);
            }
        }
    }
}