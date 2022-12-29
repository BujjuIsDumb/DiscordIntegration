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

namespace DiscordIntegration.Args
{
    /// <summary>
    ///     The event arguments for the <see cref="RpcClient.JoinRequestReceived"/> event.
    /// </summary>
    public class JoinRequestReceivedEventArgs
    {
        internal JoinRequestReceivedEventArgs(ulong userId, string username, int discriminator, string avatarUrl)
        {
            UserId = userId;
            Username = username;
            Discriminator = discriminator;
            AvatarUrl = avatarUrl;
        }

        /// <summary>
        ///     Gets the user's <see href="https://en.wikipedia.org/wiki/Snowflake_ID">Snowflake ID</see>.
        /// </summary>
        public ulong UserId { get; }

        /// <summary>
        ///     Gets the user's username.
        /// </summary>
        public string Username { get; }

        /// <summary>
        ///     Gets the user's discriminator; the 4 digit number that comes after their username.
        /// </summary>
        public int Discriminator { get; }

        /// <summary>
        ///     Gets the user's avatar URL.
        /// </summary>
        public string AvatarUrl { get; }
    }
}