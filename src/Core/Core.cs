using System;
using Microsoft.Extensions.Configuration;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using Microsoft.Extensions.DependencyInjection;

namespace NITRA_BOT_TWITCH
{
    public class Core
    {
        private TwitchClient _client;
        public TwitchClient Client {
            get => _client;
        }
        private readonly IServiceProvider _services;
        private readonly IConfigurationRoot _config;

        public Core(IServiceProvider services)
        {
            _services = services;
            _config = services.GetRequiredService<IConfigurationRoot>();

            var credentials = new ConnectionCredentials(_config["username"], _config["secret"]);

            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            var customClient = new WebSocketClient(clientOptions);

            _client = new TwitchClient(customClient);

            _client.Initialize(credentials, "IDPBBrisa");

            _client.Connect();
        }
    }
}
