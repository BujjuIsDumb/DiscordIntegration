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

using DiscordIntegration.Entities;

namespace DiscordIntegration
{
    /// <summary>
    ///     A client that can send messages to multiple webhooks at once.
    /// </summary>
    [Obsolete("This class is deprecated and will be removed in a future version. Please use a list of WebhookClient objects instead.")]
    public class MultiWebhookClient : IDisposable
    {
        /// <summary>
        ///     The clients used to send messages to the webhooks.
        /// </summary>
        private List<WebhookClient> _clients;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultiWebhookClient"/> class.
        /// </summary>
        /// <param name="webhookUrls">The webhook URLs.</param>
        public MultiWebhookClient(params string[] webhookUrls)
        {
            _clients = new List<WebhookClient>();
            WebhookUrls = webhookUrls.ToList();
        }

        /// <summary>
        ///     Gets or sets the webhook URLs.
        /// </summary>
        public List<string> WebhookUrls
        {
            get => _clients.Select(x => x.WebhookUrl).ToList();
            set
            {
                _clients.Clear();
                foreach (var url in value)
                {
                    _clients.Add(new WebhookClient(url));
                }
            }
        }

        /// <summary>
        ///     Executes the webhooks.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="profile">The webhook profile override to use.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ExecuteAsync(WebhookMessage message, WebhookProfile profile = null)
        {
            foreach (var client in _clients)
                await client.ExecuteAsync(message, profile);
        }

        /// <summary>
        ///     Executes the webhooks, with different profiles for each.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="profiles">A <see cref="Dictionary{TKey, TValue}"/> of webhook profiles to use; keyed by index in <see cref="WebhookUrls"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ExecuteAsync(WebhookMessage message, Dictionary<int, WebhookProfile> profiles = null)
        {
            for (var i = 0; i < _clients.Count; i++)
            {
                if (profiles.TryGetValue(i, out var profile))
                    await _clients[i].ExecuteAsync(message, profile);
                else
                    await _clients[i].ExecuteAsync(message);
            }
        }

        /// <summary>
        ///     Executes the webhooks, with different messages for each.
        /// </summary>
        /// <param name="messages">A <see cref="Dictionary{TKey, TValue}"/> of messages to send; keyed by index in <see cref="WebhookUrls"/>.</param>
        /// <param name="profile">The webhook profile override to use.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ExecuteAsync(Dictionary<int, WebhookMessage> messages, WebhookProfile profile = null)
        {
            for (var i = 0; i < _clients.Count; i++)
            {
                if (messages.TryGetValue(i, out var message))
                    await _clients[i].ExecuteAsync(message, profile);
            }
        }

        /// <summary>
        ///     Executes the webhooks, with different messages and profiles for each.
        /// </summary>
        /// <param name="messages">A <see cref="Dictionary{TKey, TValue}"/> of messages to send; keyed by index in <see cref="WebhookUrls"/>.</param>
        /// <param name="profiles">A <see cref="Dictionary{TKey, TValue}"/> of webhook profiles to use; keyed by index in <see cref="WebhookUrls"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ExecuteAsync(Dictionary<int, WebhookMessage> messages, Dictionary<int, WebhookProfile> profiles = null)
        {
            for (var i = 0; i < _clients.Count; i++)
            {
                if (messages.TryGetValue(i, out var message))
                {
                    if (profiles.TryGetValue(i, out var profile))
                        await _clients[i].ExecuteAsync(message, profile);
                    else
                        await _clients[i].ExecuteAsync(message);
                }
            }
        }

        /// <summary>
        ///     Executes a single webhook.
        /// </summary>
        /// <param name="index">The index of the webhook in <see cref="WebhookUrls"/>.</param>
        /// <param name="message">The message to send.</param>
        /// <param name="profile">The webhook profile override to use.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when <paramref name="index"/> is greater than the length of <see cref="WebhookUrls"/>.</exception>
        public async Task ExecuteSingleWebhook(int index, WebhookMessage message, WebhookProfile profile = null)
        {
            if (index < 0 || index >= _clients.Count)
                throw new IndexOutOfRangeException("Index is out of range of the number of webhooks.");

            await _clients[index].ExecuteAsync(message, profile);
        }

        public void Dispose()
        {
            foreach (var client in _clients)
                client.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}