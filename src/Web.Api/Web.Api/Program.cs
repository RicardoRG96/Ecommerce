using Asp.Versioning.ApiExplorer;
using Infrastructure;
using Web.Api;
using Web.Api.OpenApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration)
    .AddSwagger();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

        foreach (ApiVersionDescription description in descriptions)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.ShowExtensions();
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
