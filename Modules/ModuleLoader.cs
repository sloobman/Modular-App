using System.Reflection;

public class ModuleLoader
{
    public List<IModule> LoadModules()
    {
        return Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IModule)Activator.CreateInstance(t)!)
            .ToList();
    }
}
