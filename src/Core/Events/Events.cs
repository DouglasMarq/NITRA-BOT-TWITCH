using System;
using System.Linq;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace NITRA_BOT_TWITCH
{
    public class Events
    {
        private readonly IServiceProvider _services;
        private readonly BadwordsList _badwords;
        private readonly Core _core;

        public Events(IServiceProvider services)
        {
            _services = services;
            _badwords = services.GetRequiredService<BadwordsList>();
            _core = services.GetRequiredService<Core>();

            _core.Client.OnLog += Client_OnLog;
            _core.Client.OnJoinedChannel += Client_OnJoinedChannel;
            _core.Client.OnMessageReceived += Client_OnMessageReceived;
            _core.Client.OnWhisperReceived += Client_OnWhisperReceived;
            _core.Client.OnNewSubscriber += Client_OnNewSubscriber;
            _core.Client.OnConnected += Client_OnConnected;
        }

         private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected as {e.BotUsername} to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Starting ChatBot.");
            _core.Client.SendMessage(e.Channel, "Starting ChatBot.");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if(_badwords.Badwords.FirstOrDefault(badword => badword.Contains(e.ChatMessage.Message)) != null) {
                _core.Client.TimeoutUser(e.ChatMessage.Channel, e.ChatMessage.Username, TimeSpan.FromMinutes(5), "Ôh, Palavreado pode aqui não rapaz! 5 minutinhos calado.");
                _core.Client.SendMessage(e.ChatMessage.Channel, $"Ôh, @{e.ChatMessage.Username}, Palavreado pode aqui não rapaz! 5 minutinhos calado.");
            }
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            _core.Client.SendWhisper(e.WhisperMessage.Username, "Hey! You are losing your time whispering me. Send anything at IDPBBrisa's chat.");
        }

        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
                _core.Client.SendMessage(e.Channel, $"{e.Subscriber.DisplayName} Obrigado por ser inscrever! Você acaba de receber 500 pontos! Muito gentil da sua parte usar seu prime aqui :D");
            else
                _core.Client.SendMessage(e.Channel, $"{e.Subscriber.DisplayName} Obrigado por ser inscrever! Você acaba de receber 500 pontos!");
        }
    }
}
