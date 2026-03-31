using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class DependencyTests
{
    [Fact]
    public void Should_Order_Modules_Correctly()
    {
        // Arrange
        var modules = new List<IModule>
        {
            new LoggingModule(),
            new DataModule(),
            new ValidationModule(),
            new ReportModule()
        };

        var resolver = new DependencyResolver();

        // Act
        var ordered = resolver.Resolve(modules);
        var names = ordered.Select(m => m.Name).ToList();

        // Assert
        Assert.Equal(new[] { "Logging", "Data", "Validation", "Report" }, names);
    }

    [Fact]
    public void Should_Order_Modules_Correctly_For_Complex_Graph()
    {
        // Arrange
        var modules = new List<IModule>
        {
            new TopologyA(),
            new TopologyB(),
            new TopologyC(),
            new TopologyD()
        };

        var resolver = new DependencyResolver();

        // Act
        var ordered = resolver.Resolve(modules);
        var names = ordered.Select(m => m.Name).ToList();

        // Assert
        Assert.Equal(new[] { "A", "B", "C", "D" }, names);
    }

    [Fact]
    public void Should_Throw_When_Dependency_Missing()
    {
        // Arrange
        var modules = new List<IModule>
        {
            new FakeModule()
        };

        var resolver = new DependencyResolver();

        // Act + Assert
        var ex = Assert.Throws<Exception>(() => resolver.Resolve(modules));

        Assert.Contains("Отсутствует модуль", ex.Message);
    }

    [Fact]
    public void Should_Throw_On_Circular_Dependency()
    {
        // Arrange
        var modules = new List<IModule>
        {
            new ModuleA(),
            new ModuleB()
        };

        var resolver = new DependencyResolver();

        // Act + Assert
        var ex = Assert.Throws<Exception>(() => resolver.Resolve(modules));

        Assert.Contains("Циклическая зависимость", ex.Message);
    }

    // ===== ВСПОМОГАТЕЛЬНЫЕ МОДУЛИ ДЛЯ ТЕСТОВ =====

    class FakeModule : IModule
    {
        public string Name => "Fake";
        public IEnumerable<string> Dependencies => new[] { "Unknown" };

        public void RegisterServices(IServiceCollection services) { }
        public void Initialize(IServiceProvider provider) { }
    }

    class ModuleA : IModule
    {
        public string Name => "A";
        public IEnumerable<string> Dependencies => new[] { "B" };

        public void RegisterServices(IServiceCollection services) { }
        public void Initialize(IServiceProvider provider) { }
    }

    class ModuleB : IModule
    {
        public string Name => "B";
        public IEnumerable<string> Dependencies => new[] { "A" };

        public void RegisterServices(IServiceCollection services) { }
        public void Initialize(IServiceProvider provider) { }
    }

    class ModuleC : IModule
    {
        public string Name => "C";
        public IEnumerable<string> Dependencies => new[] { "A" };

        public void RegisterServices(IServiceCollection services) { }
        public void Initialize(IServiceProvider provider) { }
    }

    class ModuleD : IModule
    {
        public string Name => "D";
        public IEnumerable<string> Dependencies => new[] { "B", "C" };

        public void RegisterServices(IServiceCollection services) { }
        public void Initialize(IServiceProvider provider) { }
    }

    class TopologyA : IModule
    {
        public string Name => "A";
        public IEnumerable<string> Dependencies => Array.Empty<string>();

        public void RegisterServices(IServiceCollection services) { }
        public void Initialize(IServiceProvider provider) { }
    }

    class TopologyB : IModule
    {
        public string Name => "B";
        public IEnumerable<string> Dependencies => new[] { "A" };

        public void RegisterServices(IServiceCollection services) { }
        public void Initialize(IServiceProvider provider) { }
    }

    class TopologyC : IModule
    {
        public string Name => "C";
        public IEnumerable<string> Dependencies => new[] { "A" };

        public void RegisterServices(IServiceCollection services) { }
        public void Initialize(IServiceProvider provider) { }
    }

    class TopologyD : IModule
    {
        public string Name => "D";
        public IEnumerable<string> Dependencies => new[] { "B", "C" };

        public void RegisterServices(IServiceCollection services) { }
        public void Initialize(IServiceProvider provider) { }
    }
}