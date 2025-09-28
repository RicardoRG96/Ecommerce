using Infrastructure;
using Web.Api;
using Web.Api.Extensions;
using Web.Api.OpenApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGenWithAuth();

builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration)
    .AddSwagger()
    .AddApiVersioning();

WebApplication app = builder.Build();

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
