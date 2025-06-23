using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace SalesReports.Tests.Integration.Configurations;

public class BaseTest
{
    protected WebApplicationFactory<Program> Factory;
    protected HttpClient Client;
    protected IServiceScope Scope;
    protected IServiceProvider Services;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                });
            });

        Client = Factory.CreateClient();
    }

    [SetUp]
    public void SetUp()
    {
        Scope = Factory.Services.CreateScope();
        Services = Scope.ServiceProvider;
    }

    [TearDown]
    public void TearDown()
    {
        Scope?.Dispose();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Client?.Dispose();
        Factory?.Dispose();
    }

    protected T GetService<T>() where T : notnull
    {
        return Services.GetRequiredService<T>();
    }
}
