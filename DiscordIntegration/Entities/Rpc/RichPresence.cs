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

namespace DiscordIntegration.Entities.Rpc
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
        ///     Gets or sets the timestamp information of this rich presence.
        /// </summary>
        public RichPresenceTimestamp Timestamp { get; set; }

        /// <summary>
        ///     Gets or sets the large image information of this rich presence.
        /// </summary>
        public RichPresenceMedia LargeImage { get; set; }

        /// <summary>
        ///     Gets or sets the small image information of this rich presence.
        /// </summary>
        public RichPresenceMedia SmallImage { get; set; }

        /// <summary>
        ///     Gets or sets the party information of this rich presence.
        /// </summary>
        public RichPresenceParty Party { get; set; }

        /// <summary>
        ///     Gets or sets whether the match is in progress.
        /// </summary>
        public bool InProgress { get; set; }

        internal string JoinSecret { get; set; }

        internal string MatchSecret { get; set; } = Guid.NewGuid().ToString();

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
        ///     Adds timestamp information to this RPC.
        /// </summary>
        /// <param name="timestamp">The timestamp to add.</param>
        /// <returns>This RPC.</returns>
        public RichPresence WithTimestamp(RichPresenceTimestamp timestamp)
        {
            Timestamp = timestamp;
            return this;
        }

        /// <summary>
        ///     Adds large image information to this RPC.
        /// </summary>
        /// <param name="largeImage">The large image to add.</param>
        /// <returns>This RPC.</returns>
        public RichPresence WithLargeImage(RichPresenceMedia largeImage)
        {
            LargeImage = largeImage;
            return this;
        }

        /// <summary>
        ///     Adds small image information to this RPC.
        /// </summary>
        /// <param name="smallImage">The small image to add.</param>
        /// <returns>This RPC.</returns>
        public RichPresence WithSmallImage(RichPresenceMedia smallImage)
        {
            SmallImage = smallImage;
            return this;
        }

        /// <summary>
        ///     Adds party information to this RPC.
        /// </summary>
        /// <param name="party">The party to add.</param>
        /// <returns>This RPC.</returns>
        public RichPresence WithParty(RichPresenceParty party)
        {
            Party = party;
            return this;
        }

        /// <summary>
        ///     Adds a join button to this RPC.
        /// </summary>
        /// <returns>This RPC.</returns>
        public RichPresence WithJoinButton()
        {
            JoinSecret = Guid.NewGuid().ToString();
            return this;
        }

        /// <summary>
        ///     Makes the match in progress.
        /// </summary>
        /// <returns>This RPC.</returns>
        public RichPresence AsInProgress()
        {
            InProgress = true;
            return this;
        }

        internal Activity ToActivity()
        {
            var activity = new Activity
            {
                State = State,
                Details = Details,
                Instance = InProgress,
                Secrets =
                {
                    Match = MatchSecret,
                    Join = JoinSecret
                }
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