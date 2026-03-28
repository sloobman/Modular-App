using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
List<string> moduleNames;

try
{
    moduleNames = ModuleLoader.LoadModuleNames(configPath);
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
    return;
}

var loader = new ModuleLoader();
List<IModule> modules;

try
{
    modules = loader.LoadModules(moduleNames);
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
    return;
}

var resolver = new DependencyResolver();
List<IModule> ordered;

try
{
    ordered = resolver.Resolve(modules);
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
    return;
}

var runner = new ModuleRunner();
runner.Run(ordered);