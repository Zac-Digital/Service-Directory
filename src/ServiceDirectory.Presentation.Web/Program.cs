using GovUk.Frontend.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NSwag;
using ServiceDirectory.Application.Database.Commands;
using ServiceDirectory.Application.Postcode.Queries;
using ServiceDirectory.Application.Services.Queries;
using ServiceDirectory.Infrastructure.Data;
using ServiceDirectory.Infrastructure.Postcode;
using ServiceDirectory.Presentation.Web.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddGovUkFrontend();

builder.Services.AddHttpClient<IPostcodeClient, PostcodeClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ExternalServices:PostcodesIo")!);
});

builder.Services.AddTransient<IPostcodeQuery, PostcodeQuery>();
builder.Services.AddTransient<IServiceQuery, ServiceQuery>();

if (builder.Environment.IsDevelopment())
{
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

    builder.Services.AddTransient<IMockDataCommand, MockDataCommand>();
    builder.Services.AddSingleton<MinimalServiceEndpoints>();
}

builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();

    ApplicationDbContext applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await applicationDbContext.Database.MigrateAsync();

    scope.ServiceProvider.GetRequiredService<MinimalServiceEndpoints>().Register(app);

    app.UseOpenApi();
    app.MapOpenApi();
    app.UseSwaggerUi();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

await app.RunAsync();

public abstract partial class Program;