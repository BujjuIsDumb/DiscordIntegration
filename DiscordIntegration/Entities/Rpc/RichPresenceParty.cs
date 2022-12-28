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

namespace DiscordIntegration.Entities.Rpc
{
    /// <summary>
    ///     Party information for <see cref="RichPresence"/> objects.
    /// </summary>
    public sealed class RichPresenceParty
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RichPresenceParty"/> class.
        /// </summary>
        /// <param name="currentSize">The current size of the party.</param>
        /// <param name="maxSize">The maximum size of the party.</param>
        public RichPresenceParty(int currentSize, int maxSize)
        {
            CurrentSize = currentSize;
            MaxSize = maxSize;
        }

        /// <summary>
        ///     Gets or sets the current size of the party.
        /// </summary>
        public int CurrentSize { get; set; }

        /// <summary>
        ///     Gets or sets the maximum size of the party.
        /// </summary>
        public int MaxSize { get; set; }

        internal string Id { get; set; } = Guid.NewGuid().ToString();
    }
}