Console.WriteLine("Twitchlib API demo");

TwitchLib.Api.TwitchAPI api = new TwitchLib.Api.TwitchAPI();
api.Settings.ClientId = Resources.client_id;;
api.Settings.Secret = Resources.secret;

var token = await api.Auth.GetAccessTokenAsync();
api.Settings.AccessToken = token;

string channel_name = "luisllamas_es";
ExtFollowerService service = new ExtFollowerService(api);
var followersCount = await service.GetFollowersCountAsync(channel_name);

Console.Write(followersCount);
Console.ReadLine();