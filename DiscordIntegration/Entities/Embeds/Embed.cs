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

namespace DiscordIntegration.Entities.Embeds
{
    /// <summary>
    ///     Represents a Discord embed.
    /// </summary>
    public sealed class Embed
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("color")]
        [JsonConverter(typeof(EmbedColor.Converter))]
        public EmbedColor Color { get; set; }

        [JsonPropertyName("footer")]
        public EmbedFooter Footer { get; set; }

        [JsonPropertyName("image")]
        public EmbedMedia Image { get; set; }

        [JsonPropertyName("thumbnail")]
        public EmbedMedia Thumbnail { get; set; }

        [JsonPropertyName("author")]
        public EmbedAuthor Author { get; set; }

        [JsonPropertyName("fields")]
        public List<EmbedField> Fields { get; set; }

        #region Chain Construction Methods
        public Embed WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public Embed WithDescription(string description)
        {
            Description = description;
            return this;
        }

        public Embed WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public Embed WithTimestamp(DateTime timestamp)
        {
            Timestamp = timestamp;
            return this;
        }

        public Embed WithColor(EmbedColor color)
        {
            Color = color;
            return this;
        }

        public Embed WithFooter(EmbedFooter footer)
        {
            Footer = footer;
            return this;
        }

        public Embed WithImage(EmbedMedia image)
        {
            Image = image;
            return this;
        }

        public Embed WithThumbnail(EmbedMedia thumbnail)
        {
            Thumbnail = thumbnail;
            return this;
        }

        public Embed WithAuthor(EmbedAuthor author)
        {
            Author = author;
            return this;
        }

        public Embed AddField(EmbedField field)
        {
            Fields ??= new List<EmbedField>();

            Fields.Add(field);
            return this;
        }

        public Embed AddFields(params EmbedField[] fields)
        {
            Fields ??= new List<EmbedField>();

            Fields.AddRange(fields);
            return this;
        }

        public Embed AddFields(IEnumerable<EmbedField> fields)
        {
            Fields ??= new List<EmbedField>();

            Fields.AddRange(fields);
            return this;
        }
        #endregion
    }
}