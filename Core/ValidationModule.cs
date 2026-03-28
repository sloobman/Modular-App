using Microsoft.Extensions.DependencyInjection;

public class ValidationModule : IModule
{
    public string Name => "Validation";
    public IEnumerable<string> Dependencies => new[] { "Logging" };

    public void RegisterServices(IServiceCollection services) { }

    public void Initialize(IServiceProvider provider)
    {
        var logger = provider.GetRequiredService<ILoggerService>();
        logger.Log("Validation executed");
    }
}
