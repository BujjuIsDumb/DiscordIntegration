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

using DiscordIntegration.Entities.Rpc;
using DiscordIntegration.Exceptions;
using DiscordIntegration.SdkWrapper;

namespace DiscordIntegration.Rpc
{
    /// <summary>
    ///     A client for the Discord RPC.
    /// </summary>
    public class RpcClient : IDisposable
    {
        /// <summary>
        ///     The Discord client used to create the RPC.
        /// </summary>
        private Discord _client;

        /// <summary>
        ///     Whether the callbacks have been started.
        /// </summary>
        private bool _started;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RpcClient"/> class.
        /// </summary>
        /// <param name="appId">The application ID/client ID from the <see cref="https://discord.com/developers">Discord Developer Portal</see></param>
        /// <param name="rpc">The Rich Presence to use.</param>
        public RpcClient(ulong appId)
        {   
            if (!File.Exists(".\\discord_game_sdk.dll"))
                throw new FileNotFoundException("The Discord Game SDK was not found. Please make sure it is in the same directory as the executable, with the name \'discord_game_sdk.dll\'.");

            _client = new Discord((long)appId, (ulong)CreateFlags.NoRequireDiscord);
        }

        /// <summary>
        ///     Starts this RPC client.
        /// </summary>
        /// <param name="rpc">The RPC to use.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when the client has already been started.</exception>
        /// <exception cref="RpcFailedException">Thrown when Discord returns an error.</exception>
        public async Task StartAsync(RichPresence rpc)
        {
            if (_started)
                throw new Exception("This client has already been started.");

            _client.GetActivityManager().UpdateActivity(rpc.ToActivity(), result =>
            {
                if (result != Result.Ok)
                    throw new RpcFailedException(result);
            });

            _started = true;

            while (true)
            {
                _client.RunCallbacks();
                await Task.Delay(1000 / 60);
            }
        }

        public void Dispose()
        {
            if (_started)
            {
                _client.GetActivityManager().ClearActivity(result =>
                {
                    if (result != Result.Ok)
                        throw new RpcFailedException(result);
                });
            }

            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}