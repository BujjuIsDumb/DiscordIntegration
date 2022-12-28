// This is a slightly modified version of the Discord Game SDK.
// More info: https://discord.com/developers/docs/game-sdk/sdk-starter-guide

using System.Text;

namespace DiscordIntegration.Rpc.SdkWrapper
{
    internal partial class LobbyManager
    {
        public IEnumerable<User> GetMemberUsers(long lobbyID)
        {
            int memberCount = MemberCount(lobbyID);
            var members = new List<User>();
            for (int i = 0; i < memberCount; i++)
            {
                members.Add(GetMemberUser(lobbyID, GetMemberUserId(lobbyID, i)));
            }

            return members;
        }

        public void SendLobbyMessage(long lobbyID, string data, SendLobbyMessageHandler handler) => SendLobbyMessage(lobbyID, Encoding.UTF8.GetBytes(data), handler);
    }
}
