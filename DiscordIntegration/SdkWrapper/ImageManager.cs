// This is a slightly modified version of the Discord Game SDK.
// More info: https://discord.com/developers/docs/game-sdk/sdk-starter-guide

namespace DiscordIntegration.SdkWrapper
{
    internal partial struct ImageHandle
    {
        public static ImageHandle User(long id) => User(id, 128);

        public static ImageHandle User(long id, uint size)
        {
            return new ImageHandle
            {
                Type = ImageType.User,
                Id = id,
                Size = size,
            };
        }
    }

    internal partial class ImageManager
    {
        public void Fetch(ImageHandle handle, FetchHandler callback) => Fetch(handle, false, callback);

        public byte[] GetData(ImageHandle handle)
        {
            var dimensions = GetDimensions(handle);
            byte[] data = new byte[dimensions.Width * dimensions.Height * 4];
            GetData(handle, data);
            return data;
        }
    }
}
