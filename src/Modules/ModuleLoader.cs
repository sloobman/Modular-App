using System.IO;
using System.Reflection;
using System.Text.Json;

public class ModuleLoader
{
    private readonly string _modulesDirectory;

    public ModuleLoader(string? modulesDirectory = null)
    {
        _modulesDirectory = modulesDirectory ?? Path.Combine(AppContext.BaseDirectory, "modules");
    }

    public static List<string> LoadModuleNames(string configPath)
    {
        if (!File.Exists(configPath))
            throw new Exception($"Файл настроек не найден: {configPath}");

        var json = File.ReadAllText(configPath);
        var settings = JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (settings?.Modules == null || settings.Modules.Count == 0)
            throw new Exception("Список модулей пуст.");

        return settings.Modules;
    }

    public List<IModule> LoadModules(IEnumerable<string> moduleNames)
    {
        var assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };

        if (Directory.Exists(_modulesDirectory))
        {
            foreach (var file in Directory.GetFiles(_modulesDirectory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    if (!assemblies.Contains(assembly))
                        assemblies.Add(assembly);
                }
                catch
                {
                    // Игнорируем недопустимые файлы или библиотеки, которые не удаётся загрузить
                }
            }
        }

        var availableModules = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IModule)Activator.CreateInstance(t)!)
            .ToDictionary(m => m.Name);

        var result = new List<IModule>();

        foreach (var moduleName in moduleNames)
        {
            if (!availableModules.TryGetValue(moduleName, out var module))
                throw new Exception($"Отсутствует модуль: {moduleName}");

            result.Add(module);
        }

        return result;
    }

    private class AppSettings
    {
        public List<string> Modules { get; set; } = new();
    }
}
