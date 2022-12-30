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

namespace DiscordIntegration.Entities
{
    public sealed class RichPresenceTimestamp
    {
        public RichPresenceTimestamp(DateTime timestamp, TimestampDisplayType displayType)
        {

            if (displayType == TimestampDisplayType.Left)
            {
                if (timestamp < DateTime.Now)
                    throw new ArgumentException("Timestamp must be in the future when using TimestampDisplayType.Left.", nameof(timestamp));

                End = timestamp;
            }
            else if (displayType == TimestampDisplayType.Elapsed)
            {
                if (timestamp > DateTime.Now)
                    throw new ArgumentException("Timestamp must be in the past when using TimestampDisplayType.Elapsed.", nameof(timestamp));

                Start = timestamp;
            }
        }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public enum TimestampDisplayType
        {
            Left = 0,
            Elapsed = 1
        }
    }
}