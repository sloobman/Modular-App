using Microsoft.Extensions.DependencyInjection;

public class ModuleRunner
{
    public void Run(List<IModule> modules)
    {
        var services = new ServiceCollection();

        foreach (var module in modules)
        {
            module.RegisterServices(services);
        }

        var provider = services.BuildServiceProvider();

        foreach (var module in modules)
        {
            Console.WriteLine($"Запуск: {module.Name}");
            module.Initialize(provider);
        }
    }
}
