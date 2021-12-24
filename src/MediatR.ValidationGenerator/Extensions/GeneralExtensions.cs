using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediatR.ValidationGenerator.Extensions
{
    public static class GeneralExtensions
    {
        /// <summary>
        /// Would prevent provided string from having duplicates
        /// </summary>
        /// <param name="nameFormatter">
        /// Receives the duplicated string and the number of times it has been repeated so far
        /// starting from 0
        /// </param>
        /// <returns>Returns correspondands between item and its unique name</returns>
        public static IEnumerable<string> PreventDuplicateNames(
            this IEnumerable<string> items,
            Func<string, int, string> nameFormatter
            )
        {
            Dictionary<string, int> visitedCounts = new Dictionary<string, int>();

            return items.Select(name =>
            {
                int repetionNumber;
                if (visitedCounts.ContainsKey(name))
                {
                    visitedCounts[name]++;
                    repetionNumber = visitedCounts[name];
                }
                else
                {
                    repetionNumber = 0;
                    visitedCounts.Add(name, repetionNumber);
                }
                return nameFormatter(name, repetionNumber);
            });
        }

        public static string GetFromGlobal(this string name, string namespaceName)
        {
            StringBuilder sb = new();
            sb.Append("global::");
            sb.Append(namespaceName);
            bool namespaceHasDot = namespaceName.EndsWith(".");
            bool nameHasDot = name.StartsWith(".");
            if (namespaceHasDot == nameHasDot)
            {
                if (namespaceHasDot)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                else
                {
                    sb.Append(".");
                }
            }
            sb.Append(name);
            if (nameHasDot)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotEmpty(this string str)
        {
            return !str.IsEmpty();
        }

        public static bool IsNotNull(this object obj)
        {
            return !(obj is null);
        }

        public static bool None<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return !collection.Any(predicate);
        }

        public static TProperty With<T, TProperty>(this T self, Func<T, TProperty> propertyFunction,
            TProperty defaultValue = default!) where T : class
        {
            TProperty result;
            if (self == default(T))
            {
                result = defaultValue;
            }
            else
            {
                result = propertyFunction(self);
            }
            return result;
        }

        public static bool NotEndsWith(this string str, string endStr)
        {
            return !str.EndsWith(endStr);
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
        {
            return source.SelectMany(x => x);
        }
    }
}