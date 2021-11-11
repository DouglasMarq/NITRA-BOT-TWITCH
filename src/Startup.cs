using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NITRA_BOT_TWITCH
{
    public class Startup
    {
        private IConfigurationRoot _config;
        public Startup()
        {
            var _builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json");

            _config = _builder.Build();

            var services = new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<BadwordsList>()
                .AddSingleton<Api>()
                .AddSingleton<Core>()
                .AddSingleton<Events>();

            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetRequiredService<BadwordsList>();
            serviceProvider.GetRequiredService<Api>();
            serviceProvider.GetRequiredService<Core>();
            serviceProvider.GetRequiredService<Events>();
        }
    }
}
