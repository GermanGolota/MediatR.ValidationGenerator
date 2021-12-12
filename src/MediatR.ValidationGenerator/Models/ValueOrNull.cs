using System;

namespace MediatR.ValidationGenerator.Models
{
    public struct ValueOrNull<T>
    {
        public bool IsNull { get; set; }
        private T? Value { get; set; }
        public string? NullMessage { get; set; }

        public bool HasValue => !IsNull;

        public static ValueOrNull<T> CreateValue(T value)
        {
            return new ValueOrNull<T>
            {
                IsNull = false,
                Value = value
            };
        }

        public static ValueOrNull<T> CreateNull(string? nullMessage = null)
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

        /// <summary>
        /// Provides safe access to the underlying value
        /// </summary>
        /// <param name="onValue">Gets called when value is not null</param>
        /// <param name="onNull">Gets called when value is null, provides null message </param>
        public void Resolve(Action<T> onValue, Action<string?>? onNull = null)
        {
            if (HasValue)
            {
                onValue(Value!);
            }
            else
            {
                onNull?.Invoke(NullMessage);
            }
        }


        /// <summary>
        /// Provides safe access to the underlying value with some resolution vaue
        /// </summary>
        /// <param name="onValue">Gets called when value is not null</param>
        /// <param name="onNull">Gets called when value is null, provides null message </param>
        public TResult Resolve<TResult>(Func<T, TResult> onValue, Func<string?, TResult> onNull)
        {
            TResult res;
            if (HasValue)
            {
                res = onValue(Value!);
            }
            else
            {
                res = onNull.Invoke(NullMessage);
            }
            return res;
        }
    }
}
