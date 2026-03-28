using Microsoft.Extensions.DependencyInjection;

public interface ILoggerService
{
    void Log(string message);
}

public class LoggerService : ILoggerService
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

public class LoggingModule : IModule
{
    public string Name => "Logging";
    public IEnumerable<string> Dependencies => new string[] { };

    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<ILoggerService, LoggerService>();
    }

    public void Initialize(IServiceProvider provider)
    {
        var logger = provider.GetRequiredService<ILoggerService>();
        logger.Log("Logging initialized");
    }
}
