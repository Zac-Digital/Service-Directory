using NSwag;
using ServiceDirectory.Application.Postcode.Queries;
using ServiceDirectory.Infrastructure.Postcode;
using ServiceDirectory.Presentation.Api.Endpoints;

namespace ServiceDirectory.Presentation.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiDocument(options =>
        {
            options.PostProcess = document =>
            {
                document.Info = new OpenApiInfo
                {
                    Version = "Version 1.0",
                    Title = "Service Directory - API",
                    Description = "The API component of the Service Directory Web Application"
                };
            };
        });

        // TODO: URLs will be stored in appsettings.json
        builder.Services.AddHttpClient<IPostcodeClient, PostcodeClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.postcodes.io");
        });

        builder.Services.AddTransient<IPostcodeQuery, PostcodeQuery>();

        builder.Services.AddSingleton<MinimalPostcodeEndpoints>();
        
        WebApplication app = builder.Build();
        
        RegisterMinimalEndpoints(app.Services.CreateScope(), app);

        app.UseOpenApi();
        app.MapOpenApi();
        app.UseSwaggerUi();
        
        app.UseHttpsRedirection();
        app.UseAuthorization();
        
        app.Run();
    }

    private static void RegisterMinimalEndpoints(IServiceScope scope, WebApplication app)
    {
        scope.ServiceProvider.GetRequiredService<MinimalPostcodeEndpoints>().Register(app);
    }
}