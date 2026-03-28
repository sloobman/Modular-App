using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

public class ModuleLoaderTests
{
    [Fact]
    public void Should_Load_Module_List_From_Settings()
    {
        var tempFile = Path.GetTempFileName();

        try
        {
            File.WriteAllText(tempFile, "{\"Modules\":[\"Logging\",\"Validation\"]}");

            var names = ModuleLoader.LoadModuleNames(tempFile);

            Assert.Equal(new[] { "Logging", "Validation" }, names);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Fact]
    public void Should_Load_Modules_From_Configuration()
    {
        var moduleNames = new List<string> { "Logging", "Validation" };
        var loader = new ModuleLoader();

        var modules = loader.LoadModules(moduleNames);

        Assert.Equal(moduleNames, modules.ConvertAll(m => m.Name));
    }

    [Fact]
    public void Should_Throw_When_Configured_Module_Missing()
    {
        var moduleNames = new List<string> { "MissingModule" };
        var loader = new ModuleLoader();

        var ex = Assert.Throws<Exception>(() => loader.LoadModules(moduleNames));

        Assert.Contains("Отсутствует модуль", ex.Message);
    }
}
