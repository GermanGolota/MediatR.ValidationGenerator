using System;

namespace MediatR.ValidationGenerator
{
    public static class DIProvider
    {
        public static Lazy<Func<Type, object>> ResolveFunction;
    }
}
