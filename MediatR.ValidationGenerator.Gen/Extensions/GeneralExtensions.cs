﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediatR.ValidationGenerator.Gen.Extensions
{
    internal static class GeneralExtensions
    {
        public static bool IsEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
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
            TProperty defaultValue = default) where T : class
        {
            if (self == default(T))
            {
                return defaultValue;
            }
            return propertyFunction(self);
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
