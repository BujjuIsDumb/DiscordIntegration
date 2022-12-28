// MIT License
//
// Copyright(c) 2022 Bujju
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using DiscordIntegration.Rpc.SdkWrapper;

namespace DiscordIntegration.Rpc.Entities
{
    /// <summary>
    ///     Represents a rich presence object.
    /// </summary>
    public sealed class RichPresence
    {
        /// <summary>
        ///     Gets or sets the state of this rich presence.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        ///     Gets or sets the details of this rich presence.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        ///     Gets or sets the timestamp of this rich presence.
        /// </summary>
        public RichPresenceTimestamp Timestamp { get; set; }

        /// <summary>
        ///     Gets or sets the large image of this rich presence.
        /// </summary>
        public RichPresenceMedia LargeImage { get; set; }

        /// <summary>
        ///     Gets or sets the small image of this rich presence.
        /// </summary>
        public RichPresenceMedia SmallImage { get; set; }

        /// <summary>
        ///     Adds a state to this RPC.
        /// </summary>
        /// <param name="state">The state to add.</param>
        /// <returns>This RPC.</returns>
        public RichPresence WithState(string state)
        {
            State = state;
            return this;
        }

        /// <summary>
        ///     Adds details to this RPC.
        /// </summary>
        /// <param name="details">The details to add.</param>
        /// <returns>This RPC.</returns>
        public RichPresence WithDetails(string details)
        {
            Details = details;
            return this;
        }

        /// <summary>
        ///     Adds a timestamp to this RPC.
        /// </summary>
        /// <param name="timestamp">The timestamp to add.</param>
        /// <returns>This RPC.</returns>
        public RichPresence WithTimestamp(RichPresenceTimestamp timestamp)
        {
            Timestamp = timestamp;
            return this;
        }

        /// <summary>
        ///     Adds a large image to this RPC.
        /// </summary>
        /// <param name="largeImage">The large image to add.</param>
        /// <returns>This RPC.</returns>
        public RichPresence WithLargeImage(RichPresenceMedia largeImage)
        {
            LargeImage = largeImage;
            return this;
        }

        /// <summary>
        ///     Adds a small image to this RPC.
        /// </summary>
        /// <param name="smallImage">The small image to add.</param>
        /// <returns>This RPC.</returns>
        public RichPresence WithSmallImage(RichPresenceMedia smallImage)
        {
            SmallImage = smallImage;
            return this;
        }
        
        /// <summary>
        ///     Converts this RPC to an <see cref="Activity"/>.
        /// </summary>
        /// <returns>This RPC as an <see cref="Activity"/></returns>
        internal Activity ToActivity()
        {
            var activity = new Activity
            {
                State = State,
                Details = Details
            };

            if (Timestamp != null)
            {
                activity.Timestamps.Start = ((DateTimeOffset)Timestamp.Start).ToUnixTimeSeconds();
                activity.Timestamps.End = ((DateTimeOffset)Timestamp.End).ToUnixTimeSeconds();
            }

            if (LargeImage != null)
            {
                activity.Assets.SmallImage = SmallImage.ImageKey;
                activity.Assets.SmallText = SmallImage.Tooltip;
            }

            if (SmallImage != null)
            {
                activity.Assets.SmallImage = SmallImage.ImageKey;
                activity.Assets.SmallText = SmallImage.Tooltip;
            }

            return activity;
        }
    }
}