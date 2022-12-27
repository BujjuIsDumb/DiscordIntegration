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

using System.Text.Json.Serialization;

namespace DiscordIntegration.Entities
{
    /// <summary>
    ///     Represents a Discord webhook profile override.
    /// </summary>
    public sealed class WebhookProfile
    {
        /// <summary>
        ///     Gets or sets the usernamename of this profile.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        ///     Gets or sets the avatar URL of this profile.
        /// </summary>
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        /// <summary>
        ///     Adds a username override to this profile.
        /// </summary>
        /// <param name="username">The username override to add.</param>
        /// <returns>This profile.</returns>
        public WebhookProfile WithUsername(string username)
        {
            Username = username;
            return this;
        }

        /// <summary>
        ///     Adds an avatar URL override to this profile.
        /// </summary>
        /// <param name="avatarUrl">The avatar URL override to add.</param>
        /// <returns>This profile.</returns>
        public WebhookProfile WithAvatarUrl(string avatarUrl)
        {
            AvatarUrl = avatarUrl;
            return this;
        }
    }
}