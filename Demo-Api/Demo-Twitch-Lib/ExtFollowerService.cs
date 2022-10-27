namespace Demo_Twitch_Lib
{
    public class ExtFollowerService : FollowerService
    {
        public ExtFollowerService(ITwitchAPI api, int checkIntervalInSeconds = 60, int queryCountPerRequest = 100, int cacheSize = 300) : base(api, checkIntervalInSeconds, queryCountPerRequest, cacheSize)
        {
        }

        /// <summary>
        /// Retrieves all followers count for the streamer/watched channel, and is asynchronous
        /// </summary>
        /// <param name="ChannelName">The channel to retrieve the followers</param>
        /// <returns>An async int</returns>
        public async Task<long> GetFollowersCountAsync(string ChannelName)
        {

            Users followers = new(_api.Settings, new BypassLimiter(), new TwitchHttpClient());
            string channelId = (await _api.Helix.Users.GetUsersAsync(logins: new() { ChannelName })).Users.FirstOrDefault()?.Id;

            try
            {
                GetUsersFollowsResponse followsResponse = await followers.GetUsersFollowsAsync(first: 1, toId: channelId);
                return followsResponse.TotalFollows;
                
            }
            catch (Exception ex)
            {
                // do nothing
            }

            return 0;
        }

        /// <summary>
        /// Retrieves all followers for the streamer/watched channel, and is asynchronous
        /// </summary>
        /// <param name="ChannelName">The channel to retrieve the followers</param>
        /// <returns>An async task with a list of 'Follow' objects.</returns>
        public async Task<List<Follow>> GetAllFollowersAsync(string ChannelName)
        {
            int MaxFollowers = 100; // use the max for bulk retrieve

            Users followers = new(_api.Settings, new BypassLimiter(), new TwitchHttpClient());

            List<Follow> allfollows = new();

            string channelId = (await _api.Helix.Users.GetUsersAsync(logins: new() { ChannelName })).Users.FirstOrDefault()?.Id;

            try
            {
                GetUsersFollowsResponse followsResponse = await followers.GetUsersFollowsAsync(first: MaxFollowers, toId: channelId);

                allfollows.AddRange(followsResponse.Follows);

                while (followsResponse?.Follows.Length == MaxFollowers && followsResponse?.Pagination.Cursor != null) // loop until the last response is less than 100; each retrieval provides 100 items at a time
                {
                    followsResponse = await followers.GetUsersFollowsAsync(after: followsResponse.Pagination.Cursor, first: MaxFollowers, toId: channelId);
                    allfollows.AddRange(followsResponse.Follows);
                }
            }
            catch (Exception ex)
            {
                // do nothing
            }

            return allfollows;
        }
    }
}