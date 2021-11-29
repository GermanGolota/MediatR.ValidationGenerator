﻿namespace MediatR.ValidationGenerator.Models
{
    public struct ValueOrNull<T>
    {
        public bool IsNull { get; set; }
        public T Value { get; set; }

        public bool HasValue => !IsNull;
        public string NullMessage { get; set; }

        public static ValueOrNull<T> CreateValue(T value)
        {
            return new ValueOrNull<T>
            {
                IsNull = false,
                Value = value
            };
        }

        public static ValueOrNull<T> CreateNull(string nullMessage = null)
        {
            return new ValueOrNull<T>
            {
                IsNull = true,
                Value = default,
                NullMessage = nullMessage
            };
        }

        public static implicit operator ValueOrNull<T>(T value)
        {
            if (value == null)
            {
                return CreateNull();
            }
            return CreateValue(value);
        }
    }
}
