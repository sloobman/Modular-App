using Microsoft.Extensions.DependencyInjection;

public class DataModule : IModule
{
    public string Name => "Data";
    public IEnumerable<string> Dependencies => new[] { "Logging" };

    public void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IDataProvider, SampleDataProvider>();
    }

    public void Initialize(IServiceProvider provider)
    {
        var logger = provider.GetRequiredService<ILoggerService>();
        var count = provider.GetRequiredService<IDataProvider>().GetRecords().ToList().Count;
        logger.Log($"Data module initialized, loaded {count} records.");
    }

    private class SampleDataProvider : IDataProvider
    {
        private readonly List<DataRecord> _records = new()
        {
            new DataRecord("1", "Alice", 150m, true),
            new DataRecord("2", "Bob", 0m, false),
            new DataRecord("3", "Charlie", 270m, true)
        };

        public IEnumerable<DataRecord> GetRecords() => _records;
    }
}
