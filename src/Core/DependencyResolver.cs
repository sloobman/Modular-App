public class DependencyResolver
{
    public List<IModule> Resolve(List<IModule> modules)
    {
        var result = new List<IModule>();
        var visited = new HashSet<string>();
        var visiting = new HashSet<string>();

        var dict = modules.ToDictionary(m => m.Name);

        foreach (var module in modules)
        {
            Visit(module, dict, visited, visiting, result);
        }

        return result;
    }

    private void Visit(
        IModule module,
        Dictionary<string, IModule> dict,
        HashSet<string> visited,
        HashSet<string> visiting,
        List<IModule> result)
    {
        if (visited.Contains(module.Name)) return;

        if (visiting.Contains(module.Name))
            throw new Exception($"Циклическая зависимость: {module.Name}");

        visiting.Add(module.Name);

        foreach (var dep in module.Dependencies)
        {
            if (!dict.ContainsKey(dep))
                throw new Exception($"Отсутствует модуль: {dep}");

            Visit(dict[dep], dict, visited, visiting, result);
        }

        visiting.Remove(module.Name);
        visited.Add(module.Name);
        result.Add(module);
    }
}
