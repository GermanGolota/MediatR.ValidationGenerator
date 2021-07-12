using FluentValidation;
using MediatR.ValidationGenerator.Gen;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ValidationGenerator
{
    public static class DiExtensions
    {
        public static IServiceCollection AddGeneratedValidators(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient,
            Func<AssemblyScanner.AssemblyScanResult, bool> filter = null
            )
        {
            services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>), lifetime));

            assemblies = assemblies.Union(new List<Assembly> { typeof(SourceGenerator).Assembly });

            services.AddValidatorsFromAssemblies(assemblies, lifetime, filter);

            return services;
        }
    }
}
