using Microsoft.Extensions.DependencyInjection;
using System.Linq;

public class ValidationModule : IModule
{
    public string Name => "Validation";
    public IEnumerable<string> Dependencies => new[] { "Logging", "Data" };

    public void RegisterServices(IServiceCollection services) { }

    public void Initialize(IServiceProvider provider)
    {
        var logger = provider.GetRequiredService<ILoggerService>();
        var records = provider.GetRequiredService<IDataProvider>().GetRecords().ToList();

        var invalid = records.Where(r => r.Amount <= 0 || string.IsNullOrWhiteSpace(r.Name)).ToList();

        if (invalid.Any())
        {
            logger.Log($"Validation found {invalid.Count} invalid records.");
            foreach (var record in invalid)
            {
                logger.Log($" - Invalid record {record.Id}: {record.Name}, amount={record.Amount}");
            }
        }
        else
        {
            logger.Log("Validation passed");
        }
    }
}
