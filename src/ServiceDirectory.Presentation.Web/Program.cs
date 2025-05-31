using GovUk.Frontend.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NSwag;
using ServiceDirectory.Application.Database.Commands;
using ServiceDirectory.Application.Postcode.Queries;
using ServiceDirectory.Infrastructure.Data;
using ServiceDirectory.Infrastructure.Postcode;
using ServiceDirectory.Presentation.Web.Endpoints;

namespace ServiceDirectory.Presentation.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        builder.Services.AddGovUkFrontend();
        
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
        builder.Services.AddTransient<IMockDataCommand, MockDataCommand>();
        
        builder.Services.AddSingleton<MinimalServiceEndpoints>();
        
        builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
        });

        WebApplication app = builder.Build();
        
        using (IServiceScope scope = app.Services.CreateScope())
        {
            if (builder.Environment.IsDevelopment())
            {
                ApplicationDbContext applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await applicationDbContext.Database.MigrateAsync();
            }
            
            scope.ServiceProvider.GetRequiredService<MinimalServiceEndpoints>().Register(app);
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseOpenApi();
        app.MapOpenApi();
        app.UseSwaggerUi();
        
        app.UseHttpsRedirection();
        
        app.UseRouting();
        app.MapStaticAssets();
        app.MapRazorPages().WithStaticAssets();

        await app.RunAsync();
    }
}
