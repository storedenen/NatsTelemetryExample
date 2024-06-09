using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NatsTelemetryExample
{
    using System;
    using Microsoft.Extensions.Hosting;
    using NATS.Client.Hosting;
    
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            
            var hostBuilder = new HostBuilder();
            
            hostBuilder.ConfigureServices(x =>
            {
                x.AddLogging(x =>
                {
                    x.AddConsole();
                });
                x.AddNats();
                x.AddHostedService<NatsClient>();
            });

            var host = hostBuilder.Build();
        
            host.Run();
        }
    }
}
