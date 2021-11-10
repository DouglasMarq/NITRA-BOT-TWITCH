using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using Microsoft.Extensions.DependencyInjection;

namespace NITRA_BOT_TWITCH
{
    public class Core
    {
        private TwitchClient _client;
        private readonly IServiceProvider _services;
        private readonly IConfigurationRoot _config;
        private readonly BadwordsList _badwords;

        public Core(IServiceProvider services)
        {
            _services = services;
            _badwords = services.GetRequiredService<BadwordsList>();
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

            _client.OnLog += Client_OnLog;
            _client.OnJoinedChannel += Client_OnJoinedChannel;
            _client.OnMessageReceived += Client_OnMessageReceived;
            _client.OnWhisperReceived += Client_OnWhisperReceived;
            _client.OnNewSubscriber += Client_OnNewSubscriber;
            _client.OnConnected += Client_OnConnected;

            _client.Connect();
        }

         private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Starting ChatBot.");
            _client.SendMessage(e.Channel, "Starting ChatBot.");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if(_badwords.Badwords.FirstOrDefault(badword => badword.Contains(e.ChatMessage.Message)) != null) {
                _client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(5), "Ôh, Palavreado pode aqui não rapaz! 5 minutinhos calado.");
                _client.SendMessage(e.ChatMessage.Channel, $"Ôh, @{e.ChatMessage.Username}, Palavreado pode aqui não rapaz! 5 minutinhos calado.");
            }
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (e.WhisperMessage.Username == "my_friend")
                _client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                _client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
            else
                _client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
        }
    }
}
