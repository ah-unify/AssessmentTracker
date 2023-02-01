using AssessmentTracker.Api;
using AssessmentTracker.Persistence;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AssessmentTracker.Integration.Testing;

public class AssessmentTrackerApplicationFactory : WebApplicationFactory<Program>,
    IAsyncLifetime // IAsyncLifetime allows setup/ teardown per test class basis
{
    private int ContainerPort = Random.Shared.Next(55000, 60000);
    private TestcontainerDatabase _postgresContainer;

    public AssessmentTrackerApplicationFactory()
    {
        // Postgres container pointing at randomized port
        _postgresContainer = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "postgres",
                Username = "root",
                Password = "password",
                Port = ContainerPort
            })
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Configures services use in the API under test
        builder.ConfigureServices(x =>
        {
            x.RemoveAll(typeof(DataContext));
            x.RemoveAll(typeof(DbContext));
            x.RemoveAll(typeof(DbContextOptions));
            x.RemoveAll(typeof(DbContextOptions<DataContext>));

            x.AddDbContext<DataContext>(x =>
                x.UseNpgsql(
                    $"Host=localhost;User ID=root;Password=password;Port={ContainerPort};Database=AssessmentTrackerLocal")
            );
        });
    }

    /// <summary>
    /// Initialises the Postgres container for the test class
    /// </summary>
    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
    }

    /// <summary>
    /// Stops the Postgres container for the test class upon completion
    /// </summary>
    public async Task DisposeAsync()
    {
        await _postgresContainer.StopAsync();
    }
}