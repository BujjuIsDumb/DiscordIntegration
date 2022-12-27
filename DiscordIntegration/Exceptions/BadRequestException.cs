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

using System.Net;

namespace DiscordIntegration.Exceptions
{
    /// <summary>
    ///     An exception that is thrown when a request to the Discord API fails.
    /// </summary>
    public class BadRequestException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BadRequestException"/> class.
        /// </summary>
        /// <param name="response">The response that caused the exception.</param>
        public BadRequestException(HttpResponseMessage response)
            : base($"The request to the Discord API failed with status code {(int)response.StatusCode} ({response.StatusCode}).") => Response = response;

        /// <summary>
        ///     Gets the response that caused the exception.
        /// </summary>
        public HttpResponseMessage Response { get; }

        /// <summary>
        ///     Gets the error message returned by the Discord API.
        /// </summary>
        public string ErrorMessage => Response.Content.ReadAsStringAsync().Result;

        /// <summary>
        ///     Gets the error code returned by the Discord API.
        /// </summary>
        public int ErrorCode => (int)Response.StatusCode;

        /// <summary>
        ///     Gets the error code returned by the Discord API.
        /// </summary>
        public HttpStatusCode HttpStatusCode => Response.StatusCode;
    }
}