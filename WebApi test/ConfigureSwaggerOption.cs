using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi_test
{
    public class ConfigureSwaggerOption : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer scheme. \r\n\r\n" +
       "Enter 'Bearer' [Space] and then your token in the text input below. \r\n\r\n" +
       "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                        {
                            Type =ReferenceType.SecurityScheme,
                            Id="Bearer"
                        },
                Scheme = "oauth2",
                Name="Bearer",
                In= ParameterLocation.Header
            },
            new List<string>()
        }
    });
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "WebApi_test",
                Version = "v1.0",
                Description = "Api to manage Villa",
                Contact = new OpenApiContact
                {
                    Name = "keke",
                    Email = "keketsokeke03@gmail.com",
                },
                TermsOfService = new Uri("https://www.example.com"),
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://www.example.com/license"),
                }
            });
            options.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "WebApi_test",
                Version = "v2.0",
                Description = "Api to manage Villa",
                Contact = new OpenApiContact
                {
                    Name = "keke",
                    Email = "keketsokeke03@gmail.com",
                },
                TermsOfService = new Uri("https://www.example.com"),
                License = new OpenApiLicense
                {
                    Name = "Example License",
                    Url = new Uri("https://www.example.com/license"),
                }
            });
        }
    }
}
