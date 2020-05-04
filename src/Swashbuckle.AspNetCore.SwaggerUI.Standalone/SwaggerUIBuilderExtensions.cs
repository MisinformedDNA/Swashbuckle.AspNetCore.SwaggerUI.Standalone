using Swashbuckle.AspNetCore.SwaggerUI;
using SwashBuckle.AspNetCore.SwaggerUI.Standalone;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerUIBuilderExtensions
    {
        public static IServiceCollection AddSwaggerUI(
            this IServiceCollection services,
            Action<SwaggerUIOptions> setupAction = null)
        {
            services.AddTransient<SwaggerUIFileService>();
            services.AddSingleton(s =>
            {
                var options = new SwaggerUIOptions();
                setupAction?.Invoke(options);
                return options;
            });

            return services;
        }
    }
}
