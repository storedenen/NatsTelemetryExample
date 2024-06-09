using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NatsTelemetryExample.PubSub;

namespace NatsTelemetryExample
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using NATS.Client.Core;
    using NATS.Client.Serializers.Json;

    public class NatsClient : BackgroundService
    {
        private readonly ILogger<NatsClient> _logger;
        private readonly NatsOpts _natsOptions;
        private readonly string? _publisherSubject;
        private NatsConnection _natsConnection;
        private INatsSub<PubSubMessageRoot> _publisherSubscription;

        public NatsClient(ILogger<NatsClient> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _publisherSubject = configuration.GetValue("NATS_PUBLISHER_SUBJECT", "publisher");
            var url = configuration.GetValue("NATS_URL", "127.0.0.1:4222");
            var clientName = configuration.GetValue("NATS_CLIENT_NAME", "NATS-Client1");
            var password = configuration.GetValue("NATS_PASSWORD", "pass");
            var username = configuration.GetValue("NATS_PASSWORD", "user");

            _natsOptions = new NatsOpts
            {
                Url = url,
                LoggerFactory = LoggerFactory.Create(x => x.AddConsole()),
                SerializerRegistry = NatsJsonSerializerRegistry.Default,
                Name = clientName,
                AuthOpts = new NatsAuthOpts()
                {
                    Password = password,
                    Username = username,
                }
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await foreach (var msg in _publisherSubscription.Msgs.ReadAllAsync(stoppingToken))
                    {
                        if (msg.Data is PubSubMessageRoot pubSub)
                        {
                            _logger.LogInformation("Subscriber received {Subject}: {MessageId}/{PublisherId}", msg.Subject, pubSub.MessageId, pubSub.PublisherId);
                        }
                        
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "An error occured");
                }
            }
            
            _logger.LogInformation("Stopping...");
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _natsConnection = new NatsConnection(_natsOptions);
            _publisherSubscription = await _natsConnection.SubscribeCoreAsync<PubSubMessageRoot>(_publisherSubject);
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            await _publisherSubscription.DisposeAsync();
            await _natsConnection.DisposeAsync();
        }
    }
}
