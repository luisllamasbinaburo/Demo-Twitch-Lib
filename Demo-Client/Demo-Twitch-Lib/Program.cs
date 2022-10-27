using Demo_Twitch_Lib;
Console.WriteLine("Twitchlib Client demo");

var bot = new BotTwitch();
bot.Start();

Console.ReadLine();