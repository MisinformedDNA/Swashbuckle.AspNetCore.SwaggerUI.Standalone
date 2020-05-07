using Microsoft.AspNetCore.Builder;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.AzureFunctions.Extensions;
using SwashBuckle.AspNetCore.SwaggerUI.Standalone;
using System.Reflection;

[assembly: FunctionsStartup(typeof(DynamicSpec.Startup))]
namespace DynamicSpec
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            var functionAssembly = Assembly.GetExecutingAssembly();
            services.AddAzureFunctionsApiProvider(functionAssembly);

            // Add Swagger Configuration
            services.AddSwaggerGen(options =>
            {
                // SwaggerDoc - API
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Example API",
                    Version = "v1",
                    Description = "Example API",
                });
            });

            builder.Services.AddTransient<SwaggerUIFileService>();
            builder.Services.AddSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/swagger.json", "v1");
            });
        }
    }
}
