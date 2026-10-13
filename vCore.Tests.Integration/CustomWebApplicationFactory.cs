using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using vCore.Application.Services;
using vCore.Domain.Tenants;
using vCore.Infrastructure.Authorization;
using vCore.Infrastructure.Persistence;

namespace vCore.Tests.Integration;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ITenantRegistrationStatusPolicy>();
            services.AddSingleton<ITenantRegistrationStatusPolicy>(new DefaultTenantRegistrationStatusPolicy(Tenant.Statuses.Active));

            services.RemoveAll<DbContextOptions<VCoreDbContext>>();
            services.RemoveAll<VCoreDbContext>();
            services.RemoveAll<IDbContextOptionsConfiguration<VCoreDbContext>>();

            services.AddDbContext<VCoreDbContext>((serviceProvider, options) =>
            {
                var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("IntegrationTest");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'ConnectionStrings:IntegrationTest' is required.");
                }

                options.UseSqlServer(connectionString);
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<VCoreDbContext>();
            dbContext.Database.EnsureCreated();
        });
    }
}
