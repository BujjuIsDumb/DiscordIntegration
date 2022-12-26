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
    /// <summary>
    ///     Represents a file to be sent to a Discord webhook.
    /// </summary>
    public sealed class WebhookFile
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WebhookFile"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="altText">The alt text for the file.</param>
        public WebhookFile(FileInfo file, string altText = null)
        {
            File = file;
            AltText = altText;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebhookFile"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="altText">The alt text for the file.</param>
        public WebhookFile(string filePath, string altText = null)
        {
            File = new FileInfo(filePath);
            AltText = altText;
        }

        /// <summary>
        ///     Gets or sets the file.
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        ///     Gets or sets the alt text for this file.
        /// </summary>
        public string AltText { get; set; }
    }
}