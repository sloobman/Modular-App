using Xunit;
using Microsoft.Extensions.DependencyInjection;

public class ModuleTests
{
    [Fact]
    public void Should_Use_DI_Container_For_Logger()
    {
        // Arrange
        var services = new ServiceCollection();

        var module = new LoggingModule();
        module.RegisterServices(services);

        // Act
        var provider = services.BuildServiceProvider();

        var service1 = provider.GetService<ILoggerService>();
        var service2 = provider.GetService<ILoggerService>();

        // Assert
        Assert.NotNull(service1);
        Assert.Equal(service1, service2); // Singleton
    }
}