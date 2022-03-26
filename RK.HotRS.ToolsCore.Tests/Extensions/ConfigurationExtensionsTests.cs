using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RK.HotRS.ToolsCore.Extensions.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture()]
    public class ConfigurationExtensionsTests
    {
        [Test()]
        public void CleanUpJSONConfigsTest()
        {
            //starting with the default configuration, add another copy of the Env1 file.
            var config = BuildConfigWithDup();
            Assert.That(config.Providers.Count() == 4);
            config.CleanUpJSONConfigs();
            Assert.That(config.Providers.Count() == 3);

        }

        [Test()]
        public void CleanUpJSONConfigsThrowsExceptionForChainedConfigurationProviders()
        {
            //starting with the default configuration, add another copy of the Env1 file.
            var config = new ConfigurationBuilder()
                .AddConfiguration(BuildConfig())
                .AddJsonFile(@"extensions\appsettings.Env1.json")
                .Build();
            Assert.That(() => config.CleanUpJSONConfigs(), Throws.TypeOf<NotImplementedException>()
                .With.Message.EqualTo("The CleanupJsonConfig extension cannot handle Chained configurations."));
        }

        [Test]
        public void DumpTest()
        {
            //starting with the default configuration, add another copy of the Env1 file.
            var config = new ConfigurationBuilder()
                .AddConfiguration(BuildConfig())
                .AddJsonFile(@"extensions\appsettings.Env1.json")
                .Build();
            var actual = config.Dump();
            Assert.That(actual.Count > 0);
            Console.WriteLine(JsonConvert.SerializeObject(actual, Formatting.Indented));
        }        

        private IConfigurationRoot BuildConfig() =>
            new ConfigurationBuilder()
                .AddJsonFile(@"extensions\appsettings.json")
                .AddJsonFile(@"extensions\appsettings.Env1.json")
                .AddJsonFile(@"extensions\appsettings.Env2.json")
                .Build();

        private IConfigurationRoot BuildConfigWithDup() =>
            new ConfigurationBuilder()
                .AddJsonFile(@"extensions\appsettings.json")
                .AddJsonFile(@"extensions\appsettings.Env1.json")
                .AddJsonFile(@"extensions\appsettings.Env2.json")
                .AddJsonFile(@"extensions\appsettings.Env1.json")
                .Build();
    }
}