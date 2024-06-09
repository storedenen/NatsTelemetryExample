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
            var hostBuilder = new HostApplicationBuilder();

            hostBuilder.Services.AddLogging(x =>
            {
                x.AddConsole();
            });
            hostBuilder.Services.AddNats();
            hostBuilder.Services.AddHostedService<NatsClient>();

            var host = hostBuilder.Build();
        
            host.Run();
        }
    }
}
