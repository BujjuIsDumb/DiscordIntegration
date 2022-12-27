﻿// MIT License
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

namespace DiscordIntegration.Entities.Embeds
{
    /// <summary>
    ///     Author information for <see cref="Embed"/> objects.
    /// </summary>
    public sealed class EmbedAuthor
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EmbedAuthor"/> class.
        /// </summary>
        /// <param name="name">The author name.</param>
        /// <param name="url">The author URL.</param>
        /// <param name="iconUrl">The author icon URL.</param>
        public EmbedAuthor(string name, string url = null, string iconUrl = null)
        {
            Name = name;
            Url = url;
            IconUrl = iconUrl;
        }

        /// <summary>
        ///     Gets or sets the author name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the author URL.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets the author icon URL.
        /// </summary>
        [JsonPropertyName("icon_url")]
        public string IconUrl { get; set; }
    }
}