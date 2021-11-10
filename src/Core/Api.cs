using System;
using Microsoft.Extensions.Configuration;
using TwitchLib.Api;
using Microsoft.Extensions.DependencyInjection;

namespace NITRA_BOT_TWITCH
{
    public class Api
    {
        private TwitchAPI _api;
        private readonly IServiceProvider _services;
        private readonly IConfigurationRoot _config;

        public Api(IServiceProvider services)
        {
            _services = services;

            _config = services.GetRequiredService<IConfigurationRoot>();

            _api = new TwitchAPI();

            // https://twitchapps.com/tmi/
            _api.Settings.ClientId = _config["token"];
            _api.Settings.Secret = _config["secret"];
            _api.Settings.AccessToken = _config["secret"];
        }
    }
}
