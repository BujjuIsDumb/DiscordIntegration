// This is a slightly modified version of the Discord Game SDK.
// More info: https://discord.com/developers/docs/game-sdk/sdk-starter-guide

namespace DiscordIntegration.Rpc.SdkWrapper
{
    internal partial class StorageManager
    {
        public IEnumerable<FileStat> Files()
        {
            int fileCount = Count();
            var files = new List<FileStat>();
            for (int i = 0; i < fileCount; i++)
            {
                files.Add(StatAt(i));
            }

            return files;
        }
    }
}
