using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;

public class ExportModule : IModule
{
    public string Name => "Export";
    public IEnumerable<string> Dependencies => new[] { "Logging", "Data" };

    public void RegisterServices(IServiceCollection services) { }

    public void Initialize(IServiceProvider provider)
    {
        var logger = provider.GetRequiredService<ILoggerService>();
        var dataProvider = provider.GetRequiredService<IDataProvider>();

        var path = Path.Combine(AppContext.BaseDirectory, "export.csv");
        var lines = new List<string> { "Id,Name,Amount,IsValid" };
        lines.AddRange(dataProvider.GetRecords().Select(r => $"{r.Id},{r.Name},{r.Amount},{r.IsValid}"));

        File.WriteAllLines(path, lines);
        logger.Log($"Export completed: {path}");
    }
}
