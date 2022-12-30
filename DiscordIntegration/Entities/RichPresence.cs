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

using DiscordIntegration.RpcCore;

namespace DiscordIntegration.Entities
{
    public sealed class RichPresence
    {
        public string State { get; set; }

        public string Details { get; set; }

        public RichPresenceTimestamp Timestamp { get; set; }

        public RichPresenceMedia LargeImage { get; set; }

        public RichPresenceMedia SmallImage { get; set; }

        public RichPresenceParty Party { get; set; }

        public RichPresence WithState(string state)
        {
            State = state;
            return this;
        }

        public RichPresence WithDetails(string details)
        {
            Details = details;
            return this;
        }

        public RichPresence WithTimestamp(RichPresenceTimestamp timestamp)
        {
            Timestamp = timestamp;
            return this;
        }

        public RichPresence WithLargeImage(RichPresenceMedia largeImage)
        {
            LargeImage = largeImage;
            return this;
        }

        public RichPresence WithSmallImage(RichPresenceMedia smallImage)
        {
            SmallImage = smallImage;
            return this;
        }

        public RichPresence WithParty(RichPresenceParty party)
        {
            Party = party;
            return this;
        }

        internal Activity ToActivity()
        {
            var activity = new Activity
            {
                State = State,
                Details = Details
            };

            if (Timestamp?.Start != null)
            {
                activity.Timestamps.Start = ((DateTimeOffset)Timestamp.Start).ToUnixTimeSeconds();
            }

            if (Timestamp?.End != null)
            {
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

            if (Party != null)
            {
                activity.Party.Id = Party.Id;
                activity.Party.Size.CurrentSize = Party.CurrentSize;
                activity.Party.Size.MaxSize = Party.MaxSize;
            }

            return activity;
        }
    }
}