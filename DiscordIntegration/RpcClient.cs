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
using DiscordIntegration.RpcCore;

namespace DiscordIntegration
{
    /// <summary>
    ///     A client for the Discord RPC.
    /// </summary>
    public class RpcClient : IDisposable
    {
        private Discord _client;

        private bool _isDisposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RpcClient" /> class.
        /// </summary>
        /// <param name="appId">Application/client ID from the <see href="https://discord.com/developers">Discord Developer Portal</see>.</param>
        /// <param name="steamId">Steam app ID if the app is on Steam.</param>
        public RpcClient(ulong appId, uint? steamId = null)
        {
            _client = new Discord((long)appId, (ulong)CreateFlags.Default);
            if (steamId != null)
                _client.GetActivityManager().RegisterSteam(steamId.Value);
        }

        /// <summary>
        ///     Starts the client.
        /// </summary>
        /// <param name="presence">The presence to start with.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the client is disposed.</exception>
        /// <exception cref="Exception">Thrown when the presence fails to update.</exception>
        public async Task StartAsync(RichPresence presence)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("RpcClient");

            _client.GetActivityManager().UpdateActivity(presence.ToActivity(), (res) =>
            {
                if (res != Result.Ok)
                    throw new Exception($"Failed to update activity: {res}");
            });

            while (!_isDisposed)
            {
                _client.RunCallbacks();
                await Task.Delay(1000 / 60);
            }
        }

        /// <summary>
        ///     Updates the presence.
        /// </summary>
        /// <param name="presence">The presence to use.</param>
        /// <exception cref="ObjectDisposedException">Thrown when the client is disposed.</exception>
        /// <exception cref="Exception">Thrown when the presence fails to update.</exception>
        public void Update(RichPresence presence)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("RpcClient");

            _client.GetActivityManager().UpdateActivity(presence.ToActivity(), (res) =>
            {
                if (res != Result.Ok)
                    throw new Exception($"Failed to update activity: {res}");
            });
        }

        /// <exception cref="Exception">Thrown when the presence fails to clear.</exception>
        public void Dispose()
        {
            _client.GetActivityManager().ClearActivity((result) =>
            {
                if (result != Result.Ok)
                    throw new Exception($"Failed to update activity. Result: {result}");
            });
            
            _client.Dispose();
            _isDisposed = true;

            GC.SuppressFinalize(this);
        }
    }
}