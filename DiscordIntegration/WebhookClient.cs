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
using System.Text.RegularExpressions;
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
            set
            {
                // Check if the URL is valid.
                if (!Regex.IsMatch(value, @"https:\/\/discord\.com\/api\/(v\d+\/)?webhooks\/\d{17,19}\/.{68}"))
                    throw new Exception("Please provide a valid webhook URL.");

                _client.BaseAddress = new Uri(value);
            }
        }

        /// <summary>
        ///     Executes this webhook.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="profile">The webhook profile override to use.</param>
        /// <returns>The message that was sent.</returns>
        /// <exception cref="BadRequestException">Thrown when the request to the Discord API fails.</exception>
        public async Task<ulong> ExecuteAsync(WebhookMessage message, WebhookProfile profile = null)
        {
            var payload = new Payload()
            {
                Content = message.Content,
                Username = profile?.Username,
                AvatarUrl = profile?.AvatarUrl,
                Tts = message.Tts,
                Embeds = message.Embeds?.ToArray()
            };

            payload.Validate();

            // Send the request.
            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json")),
                RequestUri = new Uri(WebhookUrl + "?wait=true")
            });

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return ulong.Parse(JsonDocument.Parse(responseContent).RootElement.GetProperty("id").GetString());
            }
            else
            {
                throw new BadRequestException(response);
            }
        }

        /// <summary>
        ///     Executes this webhook.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="attachment">The attachment to send.</param>
        /// <param name="profile">The webhook profile override to use.</param>
        /// <returns>The message that was sent.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="attachment"/> isn't a png, jpg, or gif file.</exception>
        /// <exception cref="BadRequestException">Thrown when the request to the Discord API fails.</exception>
        public async Task<ulong> ExecuteAsync(WebhookMessage message, WebhookAttachment attachment, WebhookProfile profile = null)
        {
            // Check if the attachment is a png, jpg, or gif file.
            if (Path.GetExtension(attachment.FileName) != ".jpg" && Path.GetExtension(attachment.FileName) != ".jpeg" && Path.GetExtension(attachment.FileName) != ".gif" && Path.GetExtension(attachment.FileName) != ".png")
                throw new ArgumentException("Attachment must be a PNG, JPG, or GIF file.", nameof(attachment));

            var payload = new Payload()
            {
                Content = message.Content,
                Username = profile?.Username,
                AvatarUrl = profile?.AvatarUrl,
                Tts = message.Tts,
                Embeds = message.Embeds?.ToArray(),
                Attachments = new[] { attachment }
            };

            payload.Validate();

            var content = new MultipartFormDataContent
            {
                { new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json")), "payload_json" }
            };

            // Add the attachment data.
            var fileContent = new ByteArrayContent(attachment.Data);
            fileContent.Headers.Add("Content-Type", $"image/{Path.GetExtension(attachment.FileName)[1..]}");
            content.Add(fileContent, "files[0]", attachment.FileName);

            // Send the request.
            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content,
                RequestUri = new Uri(WebhookUrl + "?wait=true")
            });

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return ulong.Parse(JsonDocument.Parse(responseContent).RootElement.GetProperty("id").GetString());
            }
            else
            {
                throw new BadRequestException(response);
            }
        }

        /// <summary>
        ///     Overwrites a message sent by this webhook.
        /// </summary>
        /// <param name="messageId">The Id of then message to edit.</param>
        /// <param name="newMessage">The new message to send.</param>
        /// <returns>The edited message.</returns>
        /// <exception cref="BadRequestException">Thrown when the request to the Discord API fails.</exception>
        public async Task EditMessageAsync(ulong messageId, WebhookMessage newMessage)
        {
            var payload = new Payload()
            {
                Content = newMessage.Content,
                Embeds = newMessage.Embeds?.ToArray()
            };

            payload.Validate();

            // Send the request.
            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Patch,
                Content = new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json")),
                RequestUri = new Uri(WebhookUrl + $"/messages/{messageId}")
            });

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(response);
        }

        /// <summary>
        ///     Overwrites a message sent by this webhook.
        /// </summary>
        /// <param name="messageId">The Id of then message to edit.</param>
        /// <param name="newMessage">The new message to send.</param>
        /// <returns>The edited message.</returns>
        /// <exception cref="BadRequestException">Thrown when the request to the Discord API fails.</exception>
        public async Task EditMessageAsync(ulong messageId, WebhookMessage newMessage, WebhookAttachment attachment)
        {
            if (Path.GetExtension(attachment.FileName) != ".jpg" && Path.GetExtension(attachment.FileName) != ".jpeg" && Path.GetExtension(attachment.FileName) != ".gif" && Path.GetExtension(attachment.FileName) != ".png")
                throw new ArgumentException("Attachment must be a PNG, JPG, or GIF file.", nameof(attachment));

            var payload = new Payload()
            {
                Content = newMessage.Content,
                Embeds = newMessage.Embeds?.ToArray(),
                Attachments = new[] { attachment }
            };

            payload.Validate();

            var content = new MultipartFormDataContent
            {
                { new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json")), "payload_json" }
            };

            // Add the attachment data.
            var fileContent = new ByteArrayContent(attachment.Data);
            fileContent.Headers.Add("Content-Type", $"image/{Path.GetExtension(attachment.FileName)[1..]}");
            content.Add(fileContent, "files[0]", attachment.FileName);

            // Send the request.
            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Patch,
                Content = content,
                RequestUri = new Uri(WebhookUrl + $"/messages/{messageId}")
            });

            if (!response.IsSuccessStatusCode)
                throw new BadRequestException(response);
        }

        /// <summary>
        ///     Deletes a message sent by this webhook.
        /// </summary>
        /// <param name="messageId">The ID of the message to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="BadRequestException">Thrown when the request to the Discord API fails.</exception>
        public async Task DeleteMessageAsync(ulong messageId)
        {
            // Send the request.
            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(WebhookUrl + $"/messages/{messageId}")
            });

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
            public WebhookAttachment[] Attachments { get; set; }

            /// <summary>
            ///     Checks if the payload is valid.
            /// </summary>
            /// <exception cref="ArgumentException">Thrown if the payload is invalid.</exception>
            public void Validate()
            {
                if (Content?.Length > 2000)
                    throw new ArgumentException("Content must be less than 2000 characters.", nameof(Content));

                if (Username?.Length > 80)
                    throw new ArgumentException("Username must be less than 80 characters.", nameof(Username));

                if (string.IsNullOrWhiteSpace(Content) && Embeds.Length == 0 && Attachments.Length == 0)
                    throw new ArgumentException("Content, embeds, or attachments must be provided.", nameof(Content));

                if (Embeds != null)
                {
                    if (Embeds.Any(x => x.Title.Length > 256))
                        throw new ArgumentException("Embed title must be less than 256 characters.", nameof(Embeds));

                    if (Embeds.Any(x => x.Description.Length > 4096))
                        throw new ArgumentException("Embed description must be less than 4096 characters.", nameof(Embeds));

                    if (Embeds.Any(x => x.Fields.Count > 25))
                        throw new ArgumentException("Embed must have less than 25 fields.", nameof(Embeds));

                    if (Embeds.Any(x => x.Fields.Any(y => y.Name.Length > 256)))
                        throw new ArgumentException("Embed field name must be less than 256 characters.", nameof(Embeds));

                    if (Embeds.Any(x => x.Fields.Any(y => y.Value.Length > 1024)))
                        throw new ArgumentException("Embed field value must be less than 1024 characters.", nameof(Embeds));

                    if (Embeds.Any(x => x.Footer.Text.Length > 2048))
                        throw new ArgumentException("Embed footer text must be less than 2048 characters.", nameof(Embeds));

                    if (Embeds.Any(x => x.Author.Name.Length > 256))
                        throw new ArgumentException("Embed author name must be less than 256 characters.", nameof(Embeds));

                    if (Embeds.Any(x => string.IsNullOrEmpty(x.Title) && string.IsNullOrEmpty(x.Description) && x.Fields.Count == 0 && x.Image == null && x.Thumbnail == null && x.Footer == null && x.Author == null))
                        throw new ArgumentException("Embed must have a title, description, fields, image, thumbnail, footer, or author.", nameof(Embeds));

                    if (Embeds.Any(x => x.Fields.Any(y => string.IsNullOrEmpty(y.Name) || string.IsNullOrEmpty(y.Value))))
                        throw new ArgumentException("Embed field must have a name and value.", nameof(Embeds));

                    if (Embeds.Any(x => x.Footer != null && string.IsNullOrEmpty(x.Footer.Text)))
                        throw new ArgumentException("Embed footer must have text.", nameof(Embeds));

                    if (Embeds.Any(x => x.Author != null && string.IsNullOrEmpty(x.Author.Name)))
                        throw new ArgumentException("Embed author must have a name.", nameof(Embeds));
                }
            }
        }
    }
}