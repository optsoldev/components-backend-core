using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Optsol.Components.Shared.Settings;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Service.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        private readonly SwaggerSettings _swaggerSettings;
        private readonly SecuritySettings _securitySettings;

        public AuthorizeCheckOperationFilter(SwaggerSettings swaggerSettings, SecuritySettings securitySettings)
        {
            _swaggerSettings = swaggerSettings;
            _swaggerSettings.Validate();
            _swaggerSettings.Security.Validate();

            _securitySettings = securitySettings;
            securitySettings.Validate();
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize =
                context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [
                            new OpenApiSecurityScheme 
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = _swaggerSettings.Security.Name
                                }
                            }
                        ] = new[] { _securitySettings.ApiName }
                    }
                };

            }
        }
    }
}
