// This is a slightly modified version of the Discord Game SDK.
// More info: https://discord.com/developers/docs/game-sdk/sdk-starter-guide

namespace DiscordIntegration.SdkWrapper
{
    internal partial class StoreManager
    {
        public IEnumerable<Entitlement> GetEntitlements()
        {
            int count = CountEntitlements();
            var entitlements = new List<Entitlement>();
            for (int i = 0; i < count; i++)
            {
                entitlements.Add(GetEntitlementAt(i));
            }

            return entitlements;
        }

        public IEnumerable<Sku> GetSkus()
        {
            int count = CountSkus();
            var skus = new List<Sku>();
            for (int i = 0; i < count; i++)
            {
                skus.Add(GetSkuAt(i));
            }

            return skus;
        }
    }
}
