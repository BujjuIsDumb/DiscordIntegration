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

using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using DiscordIntegration.Entities;
using DiscordIntegration.Entities.Embeds;
using DiscordIntegration.Exceptions;

namespace DiscordIntegration
{
    /// <summary>
    ///     A client for sending messages with a Discord webhook.
    /// </summary>
    public class WebhookClient : IDisposable
    {
        /// <summary>
        ///     The <see cref="HttpClient"/> used to send REST requests.
        /// </summary>
        private HttpClient _client;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebhookClient"/> class.
        /// </summary>
        /// <param name="webhookUrl">The webhook URL.</param>
        public WebhookClient(string webhookUrl)
        {
            _client = new HttpClient();
            WebhookUrl = webhookUrl;
        }

        /// <summary>
        ///     Gets or sets the webhook URL.
        /// </summary>
        public string WebhookUrl
        {
            get => _client.BaseAddress.ToString();
            set => _client.BaseAddress = new Uri(value);
        }

        /// <summary>
        ///     Executes this webhook.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="profile">The webhook profile override to use.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="BadRequestException">Thrown when the request to the Discord API fails.</exception>
        public async Task ExecuteAsync(WebhookMessage message, WebhookProfile profile = null)
        {
            var payload = new Payload()
            {
                Content = message.Content,
                Username = profile?.Username,
                AvatarUrl = profile?.AvatarUrl,
                Tts = message.Tts,
                Embeds = message.Embeds?.ToArray()
            };

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json"))
            };
            
            var response = await _client.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(response);
        }

        /// <summary>
        ///     Executes this webhook.
        /// </summary>
        /// <param name="messaage">The message to send.</param>
        /// <param name="files">The files to send.</param>
        /// <param name="profile">The webhook profile override to use.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="BadRequestException">Thrown when the request to the Discord API fails.</exception>
        public async Task ExecuteAsync(WebhookMessage messaage, List<WebhookFile> files, WebhookProfile profile = null)
        {
            var attachments = new List<PayloadAttachment>();
            var content = new MultipartFormDataContent();
            
            for (int i = 0; i < files.Count - 1; i++)
            {
                var file = files[i];

                // Add attachment.
                attachments.Add(new PayloadAttachment()
                {
                    Id = i,
                    Description = file.AltText,
                    FileName = file.File.Name,
                });

                // Add file bytes.
                var data = new byte[file.File.Length];
                await file.File.OpenRead().ReadAsync(data);
                content.Add(new ByteArrayContent(data), "file", file.File.Name);
            }

            // Add payload.
            content.Add(new StringContent(JsonSerializer.Serialize(new Payload()
            {
                Content = messaage.Content,
                Username = profile?.Username,
                AvatarUrl = profile?.AvatarUrl,
                Tts = messaage.Tts,
                Embeds = messaage.Embeds?.ToArray()
            }), new MediaTypeHeaderValue("application/json")), "payload_json");

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content
            };

            var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(response);
        }

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Represents the <see href="https://discord.com/developers/docs/resources/webhook#execute-webhook">Execute Webhook</see> payload.
        /// </summary>
        private class Payload
        {
            /// <summary>
            ///     Gets or sets the message content.
            /// </summary>
            [JsonPropertyName("content")]
            public string Content { get; set; }

            /// <summary>
            ///     Gets or sets the username override.
            /// </summary>
            [JsonPropertyName("username")]
            public string Username { get; set; }

            /// <summary>
            ///     Gets or sets the avatar URL override.
            /// </summary>
            [JsonPropertyName("avatar_url")]
            public string AvatarUrl { get; set; }

            /// <summary>
            ///     Gets or sets whether the message should be sent as a text-to-speech message.
            /// </summary>
            [JsonPropertyName("tts")]
            public bool Tts { get; set; }

            /// <summary>
            ///     Gets or sets the embeds of the message.
            /// </summary>
            [JsonPropertyName("embeds")]
            public Embed[] Embeds { get; set; }

            /// <summary>
            ///     Gets or sets the attachments of the message.
            /// </summary>
            [JsonPropertyName("attachments")]
            public PayloadAttachment[] Attachments { get; set; }
        }

        /// <summary>
        ///     Represents an attachment to send with a message.
        /// </summary>
        internal class PayloadAttachment
        {
            /// <summary>
            ///     Gets or sets the attachment ID.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            ///     Gets or sets the attachment's alt text.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            ///     Gets or sets the attachment's filename.
            /// </summary>
            public string FileName { get; set; }
        }
    }
}