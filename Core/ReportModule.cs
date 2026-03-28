public class ReportModule : IModule
{
    public string Name => "Report";
    public IEnumerable<string> Dependencies => new[] { "Validation" };

    public void RegisterServices(IServiceCollection services) { }

    public void Initialize(IServiceProvider provider)
    {
        var logger = provider.GetRequiredService<ILoggerService>();
        logger.Log("Report generated");
    }
}
