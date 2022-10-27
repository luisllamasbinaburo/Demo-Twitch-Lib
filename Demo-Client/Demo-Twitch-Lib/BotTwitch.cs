using Demo_Twitch_Lib.Properties;

namespace Demo_Twitch_Lib
{
    public class BotTwitch
    {
        TwitchClient? client;

        public BotTwitch()
        {           
        }

        private void Init()
        {
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);

            ConnectionCredentials credentials = new ConnectionCredentials(Resources.username, Resources.oauth);
            client.Initialize(credentials);
        }


        public void Start()
        {
            if (client == null) Init();

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnChatCommandReceived += Client_OnChatCommandReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;            

            client.Connect();

            string channel_to_join = "channel_name";
            client.JoinChannel(channel_to_join);
        }

        public void Stop()
        {
            client?.Disconnect();
            client = null;
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            //Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine($"Joined to {e.Channel}");
            client?.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Console.WriteLine($"Message received: {e.ChatMessage.Message}");
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            Console.WriteLine($"Command received: {e.Command.CommandText}");
        }


        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            Console.WriteLine($"Whisper received: {e.WhisperMessage.Message}");
        }
        
        private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            Console.WriteLine($"Suscribed: {e.Subscriber.DisplayName}");
        }
    }
}

