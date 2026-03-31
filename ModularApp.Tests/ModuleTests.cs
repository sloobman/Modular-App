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

    [Fact]
    public void Should_Run_Modules_And_Show_Module_Actions()
    {
        // Arrange
        var modules = new List<IModule>
        {
            new LoggingModule(),
            new DataModule(),
            new ValidationModule(),
            new ReportModule()
        };

        var runner = new ModuleRunner();
        var originalOut = Console.Out;

        try
        {
            using var writer = new StringWriter();
            Console.SetOut(writer);

            // Act
            runner.Run(modules);

            var output = writer.ToString();

            // Assert
            Assert.Contains("Запуск: Logging", output);
            Assert.Contains("Запуск: Data", output);
            Assert.Contains("Запуск: Validation", output);
            Assert.Contains("Запуск: Report", output);
            Assert.Contains("Report generated", output);
        }
        finally
        {
            Console.SetOut(originalOut);
        }
    }
}