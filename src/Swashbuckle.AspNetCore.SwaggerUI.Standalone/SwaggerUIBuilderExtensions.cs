using Swashbuckle.AspNetCore.SwaggerUI;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SwaggerUIBuilderExtensions
    {
        public static IServiceCollection AddSwaggerUI(
            this IServiceCollection services,
            Action<SwaggerUIOptions> setupAction = null)
        {
            return services.AddSingleton(s =>
            {
                var options = new SwaggerUIOptions();
                setupAction?.Invoke(options);
                return options;
            });
        }
    }
}
