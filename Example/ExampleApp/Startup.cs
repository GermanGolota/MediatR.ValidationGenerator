using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR.ValidationGenerator;
using MediatR;
using Microsoft.OpenApi.Models;

namespace ExampleApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //MediatR
            var assemblies = new[] { typeof(Startup).Assembly };
            services.AddMediatR(assemblies);
            services.AddGeneratedValidators(assemblies);
            //

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example API"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}