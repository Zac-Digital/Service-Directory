using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ServiceDirectory.Infrastructure.Data;
using Testcontainers.MsSql;

namespace ServiceDirectory.Presentation.Web.IntegrationTest;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder().WithPassword("QbLJzwGblAR3USLi").Build();
    protected HttpClient HttpClient = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using IServiceScope scope = Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();

        HttpClient = CreateDefaultClient();
        HttpClient.BaseAddress = new Uri("https://localhost:7086");
    }

    public new async Task DisposeAsync() => await _dbContainer.DisposeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? context =
                services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ApplicationDbContext));

            if (context is null) return;

            services.Remove(context);

            ServiceDescriptor[] options = services.Where(r =>
                r.ServiceType == typeof(DbContextOptions) || (r.ServiceType.IsGenericType &&
                                                              r.ServiceType.GetGenericTypeDefinition() ==
                                                              typeof(DbContextOptions<>))).ToArray();

            foreach (ServiceDescriptor option in options)
            {
                services.Remove(option);
            }
        });

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
                options.UseSqlServer(_dbContainer.GetConnectionString()));
        });
    }
}
