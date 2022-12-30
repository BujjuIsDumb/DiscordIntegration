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

namespace DiscordIntegration.Entities.Embeds
{
    public partial struct EmbedColor
    {
        public static EmbedColor Black { get; } = new EmbedColor("000000");

        public static EmbedColor Gray { get; } = new EmbedColor("808080");

        public static EmbedColor White { get; } = new EmbedColor("FEFEFE");

        public static EmbedColor Red { get; } = new EmbedColor("FF0000");

        public static EmbedColor Pink { get; } = new EmbedColor("FF40BF");

        public static EmbedColor Burgundy { get; } = new EmbedColor("800040");

        public static EmbedColor Orange { get; } = new EmbedColor("FF8000");

        public static EmbedColor Brown { get; } = new EmbedColor("804000");

        public static EmbedColor Yellow { get; } = new EmbedColor("FFFF00");

        public static EmbedColor Green { get; } = new EmbedColor("104000");

        public static EmbedColor LimeGreen { get; } = new EmbedColor("00FF00");

        public static EmbedColor Emerald { get; } = new EmbedColor("106440");

        public static EmbedColor Olive { get; } = new EmbedColor("648040");

        public static EmbedColor Blue { get; } = new EmbedColor("0000FF");

        public static EmbedColor LightBlue { get; } = new EmbedColor("00E1FF");

        public static EmbedColor Turquoise { get; } = new EmbedColor("00FFC8");

        public static EmbedColor Purple { get; } = new EmbedColor("8000FF");

        public static EmbedColor Lilac { get; } = new EmbedColor("BD8BC7");

        public static EmbedColor Periwinkle { get; } = new EmbedColor("A582FF");

        public static EmbedColor Magenta { get; } = new EmbedColor("FF00FF");

        public static EmbedColor Blurple { get; } = new EmbedColor("6064F4");

        public static EmbedColor InvisibleDark { get; } = new EmbedColor("303434");

        public static EmbedColor InvisibleLight { get; } = new EmbedColor("F8F4F4");
    }
}