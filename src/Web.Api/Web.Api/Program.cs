using Asp.Versioning;
using Asp.Versioning.Builder;
using Infrastructure;
using System.Reflection;
using Web.Api;
using Web.Api.Extensions;
using Web.Api.OpenApi;
using Web.Api.Versioning;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGenWithAuth();

builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration)
    .AddSwagger()
    .AddVersioning();

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

WebApplication app = builder.Build();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .ReportApiVersions()
                .Build();

RouteGroupBuilder versionedGroup = app
    .MapGroup("api/v{apiVersion:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
