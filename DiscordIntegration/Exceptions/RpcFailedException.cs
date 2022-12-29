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

using DiscordIntegration.Entities.Rpc;

namespace DiscordIntegration.Exceptions
{
    /// <summary>
    ///     An exception that is thrown when a Discord RPC call fails.
    /// </summary>
    public class RpcFailedException : Exception
    {   
        internal RpcFailedException(Result result) : base($"{(int)result} ({result}) {ResultDescriptions[result]}")
        {
            ErrorCode = (int)result;
            ErrorMessage = $"{result}: {ResultDescriptions[result]}";
        }

        /// <summary>
        ///     Gets the error code.
        /// </summary>
        public int ErrorCode { get; }

        /// <summary>
        ///     Gets the error message.
        /// </summary>
        public string ErrorMessage { get; }

        #region Result Descriptions
        private static Dictionary<Result, string> ResultDescriptions => new()
        {
            [Result.ServiceUnavailable] = "Discord isn't working",
            [Result.InvalidVersion] = "the SDK version may be outdated",
            [Result.LockFailed] = "an internal error on transactional operations",
            [Result.InternalError] = "something on our side went wrong",
            [Result.InvalidPayload] = "the data you sent didn't match what we expect",
            [Result.InvalidCommand] = "that's not a thing you can do",
            [Result.InvalidPermissions] = "you aren't authorized to do that",
            [Result.NotFetched] = "couldn't fetch what you wanted",
            [Result.NotFound] = "what you're looking for doesn't exist",
            [Result.Conflict] = "user already has a network connection open on that channel",
            [Result.InvalidSecret] = "activity secrets must be unique and not match party id",
            [Result.InvalidJoinSecret] = "join request for that user does not exist",
            [Result.NoEligibleActivity] = "you accidentally set an ApplicationId in your UpdateActivity() payload",
            [Result.InvalidInvite] = "your game invite is no longer valid",
            [Result.NotAuthenticated] = "the internal auth call failed for the user, and you can't do this",
            [Result.InvalidAccessToken] = "the user's bearer token is invalid",
            [Result.ApplicationMismatch] = "access token belongs to another application",
            [Result.InvalidDataUrl] = "something internally went wrong fetching image data",
            [Result.InvalidBase64] = "not valid Base64 data",
            [Result.NotFiltered] = "you're trying to access the list before creating a stable list with Filter()",
            [Result.LobbyFull] = "the lobby is full",
            [Result.InvalidLobbySecret] = "the secret you're using to connect is wrong",
            [Result.InvalidFilename] = "file name is too long",
            [Result.InvalidFileSize] = "file is too large",
            [Result.InvalidEntitlement] = "the user does not have the right entitlement for this game",
            [Result.NotInstalled] = "Discord is not installed",
            [Result.NotRunning] = "Discord is not running",
            [Result.InsufficientBuffer] = "insufficient buffer space when trying to write",
            [Result.PurchaseCanceled] = "user cancelled the purchase flow",
            [Result.InvalidGuild] = "Discord guild does not exist",
            [Result.InvalidEvent] = "the event you're trying to subscribe to does not exist",
            [Result.InvalidChannel] = "Discord channel does not exist",
            [Result.InvalidOrigin] = "the origin header on the socket does not match what you've registered (you should not see this)",
            [Result.RateLimited] = "you are calling that method too quickly",
            [Result.OAuth2Error] = "the OAuth2 process failed at some point",
            [Result.SelectChannelTimeout] = "the user took too long selecting a channel for an invite",
            [Result.GetGuildTimeout] = "took too long trying to fetch the guild",
            [Result.SelectVoiceForceRequired] = "push to talk is required for this channel",
            [Result.CaptureShortcutAlreadyListening] = "that push to talk shortcut is already registered",
            [Result.UnauthorizedForAchievement] = "your application cannot update this achievement",
            [Result.InvalidGiftCode] = "the gift code is not valid",
            [Result.PurchaseError] = "something went wrong during the purchase flow",
            [Result.TransactionAborted] = "purchase flow aborted because the SDK is being torn down",
            [Result.DrawingInitFailed] = "undocumented"
        };
        #endregion
    }
}