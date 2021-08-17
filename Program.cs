using System;
using System.IO;
using System.Threading;
using Iot.Device.DHTxx;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RaspberryTest
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            Console.WriteLine("Go go!");

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var pin = Convert.ToInt32(configuration["PinNumber"]);
            var sleepTime = Convert.ToInt32(configuration["PollWaitInMilliseconds"]);

            using (var dht = new Dht11(pin))
            {
                while(true)
                {
                    Console.Clear();

                    Console.WriteLine($"Temperature: {dht.Temperature}");
                    Console.WriteLine($"Humidity: {dht.Humidity}");

                    Thread.Sleep(sleepTime);
                }
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton(configuration);
        }
    }
}
