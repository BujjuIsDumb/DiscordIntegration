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
using System;
using System.ComponentModel.DataAnnotations;

namespace DiscordIntegration.Entities
{
    /// <summary>
    ///     Represents a Discord webhook attachment.
    /// </summary>
    public sealed class WebhookAttachment
    {
        /// <summary>
        ///     Gets or sets the attachment's filename.
        /// </summary>
        [JsonPropertyName("filename")]
        public string FileName { get; private set; }

        /// <summary>
        ///     Gets or sets the attachment's alt text for screen readers.
        /// </summary>
        [JsonPropertyName("description")]
        [MaxLength(1024)]
        public string AltText { get; set; }

        /// <summary>
        ///     Gets or sets the attachment's bytes.
        /// </summary>
        [JsonIgnore]
        public byte[] Data { get; private set; }

        /// <summary>
        ///     Gets or sets the attachment ID. Always 0, as the webhook client only supports sending one attachment.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; internal set; } = "0";

        /// <summary>
        ///     Gets a <see cref="WebhookAttachment"/> from a local file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="altText">The attachment's alt text for screen readers.</param>
        /// <param name="spoiler">Whether the attachment should be sent as a spoiler.</param>
        /// <returns>The attachment.</returns>
        public static WebhookAttachment FromFile(string filePath, string altText = null, bool spoiler = false)
        {
            return new WebhookAttachment()
            {
                FileName = (spoiler ? "SPOILER_" : null) + Path.GetFileName(filePath),
                AltText = altText,
                Data = File.ReadAllBytes(filePath)
            };
        }

        /// <summary>
        ///     Gets a <see cref="WebhookAttachment"/> from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fileName">The attachment's file name.</param>
        /// <param name="altText">The attachment's alt text for screen readers.</param>
        /// <param name="spoiler">Whether the attachment should be sent as a spoiler.</param>
        /// <returns>The attachment.</returns>
        public static async Task<WebhookAttachment> FromStreamAsync(FileStream stream, string fileName, string altText = null, bool spoiler = false)
        {
            var attachment = new WebhookAttachment()
            {
                FileName = (spoiler ? "SPOILER_" : null) + fileName,
                AltText = altText,
                Data = new byte[stream.Length]
            };

            await stream.ReadAsync(attachment.Data.AsMemory(0, (int)stream.Length));
            return attachment;
        }
    }
}