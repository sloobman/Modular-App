using Microsoft.Extensions.DependencyInjection;
using System.Linq;

public class ReportModule : IModule
{
    public string Name => "Report";
    public IEnumerable<string> Dependencies => new[] { "Validation", "Data" };

    public void RegisterServices(IServiceCollection services) { }

    public void Initialize(IServiceProvider provider)
    {
        var logger = provider.GetRequiredService<ILoggerService>();
        var records = provider.GetRequiredService<IDataProvider>().GetRecords().ToList();

        var totalAmount = records.Sum(r => r.Amount);
        var invalidCount = records.Count(r => !r.IsValid);
        var report = $"Report:\n  Records: {records.Count}\n  Total amount: {totalAmount:C}\n  Invalid records: {invalidCount}";

        Console.WriteLine(report);
        logger.Log("Report generated");
    }
}
