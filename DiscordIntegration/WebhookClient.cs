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

using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using DiscordIntegration.Entities;
using DiscordIntegration.Entities.Embeds;

namespace DiscordIntegration
{
    public class WebhookClient : IDisposable
    {
        private HttpClient _client;

        private bool _isDisposed;

        public WebhookClient(string webhookUrl)
        {
            _client = new HttpClient();
            WebhookUrl = webhookUrl;
        }

        public string WebhookUrl
        {
            get => _client.BaseAddress.ToString();
            set
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(nameof(WebhookClient));

                if (!Regex.IsMatch(value, @"https:\/\/discord\.com\/api\/(v\d+\/)?webhooks\/\d{17,19}\/.{68}"))
                    throw new Exception("Please provide a valid webhook URL.");

                _client.BaseAddress = new Uri(value);
            }
        }

        public async Task<ulong> ExecuteAsync(WebhookMessage message, WebhookProfile profile = null)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(WebhookClient));

            var payload = new Payload()
            {
                Content = message.Content,
                Embeds = message.Embeds?.ToArray(),
                Username = profile?.Username,
                AvatarUrl = profile?.AvatarUrl
            };

            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json")),
                RequestUri = new Uri(WebhookUrl + "?wait=true")
            });

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Request failed with status code {response.StatusCode}.\n\n{await response.Content.ReadAsStringAsync()}");
            
            return ulong.Parse(JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("id").GetString());
        }

        public async Task<ulong> ExecuteAsync(WebhookMessage message, WebhookAttachment attachment, WebhookProfile profile = null)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(WebhookClient));

            var payload = new Payload()
            {
                Content = message.Content,
                Embeds = message.Embeds?.ToArray(),
                Username = profile?.Username,
                AvatarUrl = profile?.AvatarUrl,
                Attachments = new[] { attachment }
            };

            var content = new MultipartFormDataContent
            {
                {
                    new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json")),
                    "payload_json"
                },
                {
                    new ByteArrayContent(attachment.FileData),
                    $"files[{attachment.Id}]",
                    attachment.FileName
                }
            };

            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content,
                RequestUri = new Uri(WebhookUrl + "?wait=true")
            });


            if (!response.IsSuccessStatusCode)
                throw new Exception($"Request failed with status code {response.StatusCode}.\n\n{await response.Content.ReadAsStringAsync()}");

            return ulong.Parse(JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("id").GetString());
        }

        public async Task<ulong> ExecuteAsync(WebhookMessage message, IEnumerable<WebhookAttachment> attachments, WebhookProfile profile = null)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(WebhookClient));

            var payload = new Payload()
            {
                Content = message.Content,
                Embeds = message.Embeds?.ToArray(),
                Username = profile?.Username,
                AvatarUrl = profile?.AvatarUrl,
                Attachments = attachments.Select(x => new WebhookAttachment(x, attachments.ToList().IndexOf(x))).ToArray()
            };

            var content = new MultipartFormDataContent()
            {
                {
                    new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json")),
                    "payload_json"
                }
            };
            
            foreach (var attachment in payload.Attachments)
                content.Add(new ByteArrayContent(attachment.FileData), $"files[{attachment.Id}]", attachment.FileName);

            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content,
                RequestUri = new Uri(WebhookUrl + "?wait=true")
            });


            if (!response.IsSuccessStatusCode)
                throw new Exception($"Request failed with status code {response.StatusCode}.\n\n{await response.Content.ReadAsStringAsync()}");

            return ulong.Parse(JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("id").GetString());
        }

        public async Task EditMessageAsync(ulong messageId, WebhookMessage newMessage)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(WebhookClient));

            var payload = new Payload()
            {
                Content = newMessage.Content,
                Embeds = newMessage.Embeds?.ToArray()
            };

            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Patch,
                Content = new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json")),
                RequestUri = new Uri(WebhookUrl + $"/messages/{messageId}")
            });

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Request failed with status code {response.StatusCode}.\n\n{await response.Content.ReadAsStringAsync()}");
        }

        public async Task EditMessageAsync(ulong messageId, WebhookMessage newMessage, IEnumerable<WebhookAttachment> newAttachments)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(WebhookClient));

            var payload = new Payload()
            {
                Content = newMessage.Content,
                Embeds = newMessage.Embeds?.ToArray(),
                Attachments = newAttachments.Select(x => new WebhookAttachment(x, newAttachments.ToList().IndexOf(x))).ToArray()
            };

            var content = new MultipartFormDataContent()
            {
                {
                    new StringContent(JsonSerializer.Serialize(payload), new MediaTypeHeaderValue("application/json")),
                    "payload_json"
                }
            };

            foreach (var attachment in payload.Attachments)
                content.Add(new ByteArrayContent(attachment.FileData), $"files[{attachment.Id}]", attachment.FileName);

            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = content,
                RequestUri = new Uri(WebhookUrl + $"/messages/{messageId}")
            });

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Request failed with status code {response.StatusCode}.");
        }

        public async Task DeleteMessageAsync(ulong messageId)
        {
            var response = await _client.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(WebhookUrl + $"/messages/{messageId}")
            });

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Request failed with status code {response.StatusCode}.\n\n{await response.Content.ReadAsStringAsync()}");
        }

        public void Dispose()
        {
            _client.Dispose();
            _isDisposed = true;

            GC.SuppressFinalize(this);
        }

        private class Payload
        {
            [JsonPropertyName("content")]
            public string Content { get; set; }

            [JsonPropertyName("username")]
            public string Username { get; set; }

            [JsonPropertyName("avatar_url")]
            public string AvatarUrl { get; set; }

            [JsonPropertyName("tts")]
            public bool Tts { get; set; }

            [JsonPropertyName("embeds")]
            public Embed[] Embeds { get; set; }

            [JsonPropertyName("attachments")]
            public WebhookAttachment[] Attachments { get; set; }
        }
    }
}