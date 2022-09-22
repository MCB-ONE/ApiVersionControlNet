using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiVersionControlNet.Config
{
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {

        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "My .Net Api restull",
                Version = description.ApiVersion.ToString(),
                Description = "This is my first API Versioning Control",
                Contact = new OpenApiContact()
                {
                    Email = "toni.salvado.rubio@gmail.com",
                    Name = "Toni Salvadó"
                }

            };

            if (description.IsDeprecated)
                info.Description += "This API Versión has been deprecated";

            return info;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // Añadir documentación de swagger para cada una de las versiones de la API
            foreach(var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }


    }
}
