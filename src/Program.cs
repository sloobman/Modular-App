using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var loader = new ModuleLoader();
var modules = loader.LoadModules();

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