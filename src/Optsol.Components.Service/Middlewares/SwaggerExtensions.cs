using Microsoft.Extensions.Configuration;
using Optsol.Components.Shared.Settings;

namespace Microsoft.AspNetCore.Builder
{
    public static class SwaggerExtensions
    {
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();
            swaggerSettings.Validate();

            var enabledSwagger = swaggerSettings.Enabled;
            if (enabledSwagger)
            {
                builder.UseSwagger();
                builder.UseSwaggerUI(
                    setup =>
                    {
                        setup.SwaggerEndpoint($"/swagger/{swaggerSettings.Version}/swagger.json", $"{swaggerSettings.Name} {swaggerSettings.Version.ToUpper()}");
                    });

            }

            return builder;
        }
    }
}
