using Microsoft.Extensions.Configuration;

namespace ConfigurationExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("ConfigFiles/mysettings.json", optional: false)
                .AddJsonFile("ConfigFiles/mysettings.optional.json", optional: true)
                .AddXmlFile("ConfigFiles/mysettings.xml")
                .AddKeyPerFile(@"C:\Users\namdo\source\repos\LearnDotNet-Samples\ConfigurationSample\ConfigFiles\KeyPerFile\", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            PrintConfiguredProviders(configuration);
            Console.WriteLine();

            PrintConfigValues(configuration);
            Console.WriteLine();

            var config = configuration.GetRequiredSection("JsonConfigSample").Get<BindingConfig>();
            Print(config);
        }

        private static void Print(BindingConfig? config)
        {
            if (config == null)
            {
                Console.WriteLine("config == null");
            }
            else
            {
                Console.WriteLine($"StringConfig: {config.StringConfig}");
                Console.WriteLine($"IntegerConfig: {config.IntegerConfig}");
                Console.WriteLine($"BoolConfig: {config.BoolConfig}");
            }
        }

        private static void PrintConfiguredProviders(IConfigurationRoot configuration)
        {
            foreach (var p in configuration.Providers)
            {
                Console.WriteLine($"Provider: {p.GetType().Name}");
            }
        }

        private static void PrintConfigValues(IConfigurationRoot configuration)
        {
            foreach (var config in configuration.AsEnumerable())
            {
                Console.WriteLine($"{config.Key} = {config.Value}");
            }
        }
    }
}
