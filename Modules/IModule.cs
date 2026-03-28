using Microsoft.Extensions.DependencyInjection;

public interface IModule
{
    string Name { get; }
    IEnumerable<string> Dependencies { get; }

    void RegisterServices(IServiceCollection services);
    void Initialize(IServiceProvider provider);
}