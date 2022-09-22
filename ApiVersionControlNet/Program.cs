using ApiVersionControlNet.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 1. Añadir HttpClient para poder hacer peticiones http en los controladores
builder.Services.AddHttpClient();

// 2. Añadir Control de Verisonado de la aplicación
builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.ReportApiVersions = true;
});

// 3. Añadir configurción al documento de veriones
builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV"; // 1.0.0, 1.1.0, etc.
    setup.SubstituteApiVersionInUrl = true; // Añadir version en la url de los controladores
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. Configurar las opciones que hemos creado para incorporarlas a la generación de la documentación de swagger
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();

// 5. Configurar los endpoints para swagger DOCS que se van a generar para cada version de la API
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // 6. Configurar los swagger DOCS 
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
              $"/swagger/{description.GroupName}/swagger.josn",
              description.GroupName.ToUpperInvariant()
            );
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
