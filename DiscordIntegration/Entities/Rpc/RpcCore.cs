// This is a heavily modified version of the Discord Game SDK.

using System.Runtime.InteropServices;

namespace DiscordIntegration.Entities.Rpc
{
    internal enum Result
    {
        Ok = 0,
        ServiceUnavailable = 1,
        InvalidVersion = 2,
        LockFailed = 3,
        InternalError = 4,
        InvalidPayload = 5,
        InvalidCommand = 6,
        InvalidPermissions = 7,
        NotFetched = 8,
        NotFound = 9,
        Conflict = 10,
        InvalidSecret = 11,
        InvalidJoinSecret = 12,
        NoEligibleActivity = 13,
        InvalidInvite = 14,
        NotAuthenticated = 15,
        InvalidAccessToken = 16,
        ApplicationMismatch = 17,
        InvalidDataUrl = 18,
        InvalidBase64 = 19,
        NotFiltered = 20,
        LobbyFull = 21,
        InvalidLobbySecret = 22,
        InvalidFilename = 23,
        InvalidFileSize = 24,
        InvalidEntitlement = 25,
        NotInstalled = 26,
        NotRunning = 27,
        InsufficientBuffer = 28,
        PurchaseCanceled = 29,
        InvalidGuild = 30,
        InvalidEvent = 31,
        InvalidChannel = 32,
        InvalidOrigin = 33,
        RateLimited = 34,
        OAuth2Error = 35,
        SelectChannelTimeout = 36,
        GetGuildTimeout = 37,
        SelectVoiceForceRequired = 38,
        CaptureShortcutAlreadyListening = 39,
        UnauthorizedForAchievement = 40,
        InvalidGiftCode = 41,
        PurchaseError = 42,
        TransactionAborted = 43,
        DrawingInitFailed = 44
    }

    internal enum CreateFlags
    {
        Default = 0,
        NoRequireDiscord = 1
    }
    
    internal enum LogLevel
    {
        Error = 1,
        Warn = 2,
        Info = 3,
        Debug = 4
    }

    internal enum ActivityPartyPrivacy
    {
        Private = 0,
        Public = 1
    }

    internal enum ActivityType
    {
        Playing = 0,
        Streaming = 1,
        Listening = 2,
        Watching = 3
    }

    internal enum ActivityActionType
    {
        Join = 1,
        Spectate = 2
    }

    internal enum ActivityJoinRequestReply
    {
        No = 0,
        Yes = 1,
        Ignore = 2
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal partial struct User
    {
        public long Id;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Username;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Discriminator;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Avatar;

        public bool Bot;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal partial struct ActivityTimestamps
    {
        public long Start;

        public long End;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal partial struct ActivityAssets
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string LargeImage;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string LargeText;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string SmallImage;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string SmallText;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal partial struct PartySize
    {
        public int CurrentSize;

        public int MaxSize;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal partial struct ActivityParty
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Id;

        public PartySize Size;

        public ActivityPartyPrivacy Privacy;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal partial struct ActivitySecrets
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Match;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Join;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Spectate;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal partial struct Activity
    {
        public ActivityType Type;

        public long ApplicationId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string State;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string Details;

        public ActivityTimestamps Timestamps;

        public ActivityAssets Assets;

        public ActivityParty Party;

        public ActivitySecrets Secrets;

        public bool Instance;

        public uint SupportedPlatforms;
    }

    internal partial class ResultException : Exception
    {
        public readonly Result Result;

        public ResultException(Result result) : base(result.ToString())
        {
            Result = result;
        }
    }

    internal partial class InternalRpcClient : IDisposable
    {
        public delegate void SetLogHookHandler(LogLevel level, string message);

        public GCHandle SelfHandle;

        public readonly nint EventsPtr;

        public readonly FFIEvents Events;

        public readonly nint ActivityEventsPtr;

        public ActivityManager.FFIEvents ActivityEvents;

        public ActivityManager ActivityManagerInstance;

        public readonly nint MethodsPtr;

        public object MethodsStructure;

        public GCHandle? setLogHook;

        public InternalRpcClient(long clientId, ulong flags)
        {
            var createParams = new FFICreateParams();
            createParams.ClientId = clientId;
            createParams.Flags = flags;
            createParams.Events = EventsPtr;
            createParams.EventData = GCHandle.ToIntPtr(SelfHandle);
            createParams.ActivityEvents = ActivityEventsPtr;
            createParams.ActivityVersion = 1;

            SelfHandle = GCHandle.Alloc(this);
            Events = new FFIEvents();
            EventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(Events));
            ActivityEvents = new ActivityManager.FFIEvents();
            ActivityEventsPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ActivityEvents));
            InitEvents(EventsPtr, ref Events);
            
            var result = DiscordCreate(3, ref createParams, out MethodsPtr);
            if (result != Result.Ok)
            {
                Dispose();
                throw new ResultException(result);
            }
        }

        public FFIMethods Methods
        {
            get => (FFIMethods)(MethodsStructure ??= Marshal.PtrToStructure(MethodsPtr, typeof(FFIMethods)));
        }

        [LibraryImport(".\\discord_game_sdk.dll")]
        public static partial Result DiscordCreate(uint version, ref FFICreateParams createParams, out nint manager);

        public void InitEvents(nint eventsPtr, ref FFIEvents events) => Marshal.StructureToPtr(events, eventsPtr, false);

        public void Dispose()
        {
            if (MethodsPtr != nint.Zero)
                Methods.Destroy(MethodsPtr);

            if (setLogHook.HasValue)
                setLogHook.Value.Free();

            SelfHandle.Free();
            Marshal.FreeHGlobal(EventsPtr);
            Marshal.FreeHGlobal(ActivityEventsPtr);

            GC.SuppressFinalize(this);
        }

        public void RunCallbacks()
        {
            var res = Methods.RunCallbacks(MethodsPtr);
            if (res != Result.Ok)
                throw new ResultException(res);
        }

        [MonoPInvokeCallback]
        public static void SetLogHookCallbackImpl(nint ptr, LogLevel level, string message)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var callback = (SetLogHookHandler)h.Target;
            callback(level, message);
        }

        public void SetLogHook(LogLevel minLevel, SetLogHookHandler callback)
        {
            if (setLogHook.HasValue)
                setLogHook.Value.Free();
            
            setLogHook = GCHandle.Alloc(callback);
            Methods.SetLogHook(MethodsPtr, minLevel, GCHandle.ToIntPtr(setLogHook.Value), SetLogHookCallbackImpl);
        }

        public ActivityManager GetActivityManager()
        {
            ActivityManagerInstance ??= new ActivityManager(Methods.GetActivityManager(MethodsPtr),ActivityEventsPtr,ref ActivityEvents);
            return ActivityManagerInstance;
        }

        [StructLayout(LayoutKind.Sequential)]
        public partial struct FFIEvents
        {
        }

        [StructLayout(LayoutKind.Sequential)]
        public partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void DestroyHandler(nint MethodsPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate Result RunCallbacksMethod(nint methodsPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void SetLogHookCallback(nint ptr, LogLevel level, [MarshalAs(UnmanagedType.LPStr)] string message);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void SetLogHookMethod(nint methodsPtr, LogLevel minLevel, nint callbackData, SetLogHookCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetApplicationManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetUserManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetImageManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetActivityManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetRelationshipManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetLobbyManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetNetworkManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetOverlayManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetStorageManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetStoreManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetVoiceManagerMethod(nint discordPtr);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate nint GetAchievementManagerMethod(nint discordPtr);

            public DestroyHandler Destroy;

            public RunCallbacksMethod RunCallbacks;

            public SetLogHookMethod SetLogHook;

            public GetApplicationManagerMethod GetApplicationManager;

            public GetUserManagerMethod GetUserManager;

            public GetImageManagerMethod GetImageManager;

            public GetActivityManagerMethod GetActivityManager;

            public GetRelationshipManagerMethod GetRelationshipManager;

            public GetLobbyManagerMethod GetLobbyManager;

            public GetNetworkManagerMethod GetNetworkManager;

            public GetOverlayManagerMethod GetOverlayManager;

            public GetStorageManagerMethod GetStorageManager;

            public GetStoreManagerMethod GetStoreManager;

            public GetVoiceManagerMethod GetVoiceManager;

            public GetAchievementManagerMethod GetAchievementManager;
        }

        [StructLayout(LayoutKind.Sequential)]
        public partial struct FFICreateParams
        {
            public long ClientId;

            public ulong Flags;

            public nint Events;

            public nint EventData;

            public nint ApplicationEvents;

            public uint ApplicationVersion;

            public nint UserEvents;

            public uint UserVersion;

            public nint ImageEvents;

            public uint ImageVersion;

            public nint ActivityEvents;

            public uint ActivityVersion;

            public nint RelationshipEvents;

            public uint RelationshipVersion;

            public nint LobbyEvents;

            public uint LobbyVersion;

            public nint NetworkEvents;

            public uint NetworkVersion;

            public nint OverlayEvents;

            public uint OverlayVersion;

            public nint StorageEvents;

            public uint StorageVersion;

            public nint StoreEvents;

            public uint StoreVersion;

            public nint VoiceEvents;

            public uint VoiceVersion;

            public nint AchievementEvents;

            public uint AchievementVersion;
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    internal partial class MonoPInvokeCallbackAttribute : Attribute
    {
    }

    internal partial class ActivityManager
    {
        public delegate void UpdateActivityHandler(Result result);

        public delegate void ClearActivityHandler(Result result);

        public delegate void SendRequestReplyHandler(Result result);

        public delegate void SendInviteHandler(Result result);

        public delegate void AcceptInviteHandler(Result result);

        public delegate void ActivityJoinHandler(string secret);

        public delegate void ActivitySpectateHandler(string secret);

        public delegate void ActivityJoinRequestHandler(ref User user);

        public delegate void ActivityInviteHandler(ActivityActionType type, ref User user, ref Activity activity);

        public readonly nint MethodsPtr;

        public object MethodsStructure;

        public event ActivityJoinHandler OnActivityJoin;

        public event ActivitySpectateHandler OnActivitySpectate;

        public event ActivityJoinRequestHandler OnActivityJoinRequest;

        public event ActivityInviteHandler OnActivityInvite;

        public FFIMethods Methods
        {
            get
            {
                MethodsStructure ??= Marshal.PtrToStructure(MethodsPtr, typeof(FFIMethods));
                return (FFIMethods)MethodsStructure;
            }
        }

        public ActivityManager(nint ptr, nint eventsPtr, ref FFIEvents events)
        {
            if (eventsPtr == nint.Zero)
                throw new ResultException(Result.InternalError);

            if (MethodsPtr == nint.Zero)
                throw new ResultException(Result.InternalError);

            InitEvents(eventsPtr, ref events);
            MethodsPtr = ptr;
        }

        public void InitEvents(nint eventsPtr, ref FFIEvents events)
        {
            events.OnActivityJoin = OnActivityJoinImpl;
            events.OnActivitySpectate = OnActivitySpectateImpl;
            events.OnActivityJoinRequest = OnActivityJoinRequestImpl;
            events.OnActivityInvite = OnActivityInviteImpl;
            Marshal.StructureToPtr(events, eventsPtr, false);
        }

        public void RegisterCommand(string command)
        {
            var res = Methods.RegisterCommand(MethodsPtr, command);
            
            if (res != Result.Ok)
                throw new ResultException(res);
        }

        public void RegisterSteam(uint steamId)
        {
            var res = Methods.RegisterSteam(MethodsPtr, steamId);
            
            if (res != Result.Ok)
                throw new ResultException(res);
        }

        [MonoPInvokeCallback]
        public static void UpdateActivityCallbackImpl(nint ptr, Result result)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var callback = (UpdateActivityHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void UpdateActivity(Activity activity, UpdateActivityHandler callback)
        {
            var wrapped = GCHandle.Alloc(callback);
            Methods.UpdateActivity(MethodsPtr, ref activity, GCHandle.ToIntPtr(wrapped), UpdateActivityCallbackImpl);
        }

        [MonoPInvokeCallback]
        public static void ClearActivityCallbackImpl(nint ptr, Result result)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var callback = (ClearActivityHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void ClearActivity(ClearActivityHandler callback)
        {
            var wrapped = GCHandle.Alloc(callback);
            Methods.ClearActivity(MethodsPtr, GCHandle.ToIntPtr(wrapped), ClearActivityCallbackImpl);
        }

        [MonoPInvokeCallback]
        public static void SendRequestReplyCallbackImpl(nint ptr, Result result)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var callback = (SendRequestReplyHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void SendRequestReply(long userId, ActivityJoinRequestReply reply, SendRequestReplyHandler callback)
        {
            var wrapped = GCHandle.Alloc(callback);
            Methods.SendRequestReply(MethodsPtr, userId, reply, GCHandle.ToIntPtr(wrapped), SendRequestReplyCallbackImpl);
        }

        [MonoPInvokeCallback]
        public static void SendInviteCallbackImpl(nint ptr, Result result)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var callback = (SendInviteHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void SendInvite(long userId, ActivityActionType type, string content, SendInviteHandler callback)
        {
            var wrapped = GCHandle.Alloc(callback);
            Methods.SendInvite(MethodsPtr, userId, type, content, GCHandle.ToIntPtr(wrapped), SendInviteCallbackImpl);
        }

        [MonoPInvokeCallback]
        public static void AcceptInviteCallbackImpl(nint ptr, Result result)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var callback = (AcceptInviteHandler)h.Target;
            h.Free();
            callback(result);
        }

        public void AcceptInvite(long userId, AcceptInviteHandler callback)
        {
            var wrapped = GCHandle.Alloc(callback);
            Methods.AcceptInvite(MethodsPtr, userId, GCHandle.ToIntPtr(wrapped), AcceptInviteCallbackImpl);
        }

        [MonoPInvokeCallback]
        public static void OnActivityJoinImpl(nint ptr, string secret)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var d = (InternalRpcClient)h.Target;
            d.ActivityManagerInstance.OnActivityJoin?.Invoke(secret);
        }

        [MonoPInvokeCallback]
        public static void OnActivitySpectateImpl(nint ptr, string secret)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var d = (InternalRpcClient)h.Target;
            d.ActivityManagerInstance.OnActivitySpectate?.Invoke(secret);
        }

        [MonoPInvokeCallback]
        public static void OnActivityJoinRequestImpl(nint ptr, ref User user)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var d = (InternalRpcClient)h.Target;
            d.ActivityManagerInstance.OnActivityJoinRequest?.Invoke(ref user);
        }

        [MonoPInvokeCallback]
        public static void OnActivityInviteImpl(nint ptr, ActivityActionType type, ref User user, ref Activity activity)
        {
            var h = GCHandle.FromIntPtr(ptr);
            var d = (InternalRpcClient)h.Target;
            d.ActivityManagerInstance.OnActivityInvite?.Invoke(type, ref user, ref activity);
        }

        [StructLayout(LayoutKind.Sequential)]
        public partial struct FFIEvents
        {
            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void ActivityJoinHandler(nint ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void ActivitySpectateHandler(nint ptr, [MarshalAs(UnmanagedType.LPStr)] string secret);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void ActivityJoinRequestHandler(nint ptr, ref User user);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void ActivityInviteHandler(nint ptr, ActivityActionType type, ref User user, ref Activity activity);

            public ActivityJoinHandler OnActivityJoin;

            public ActivitySpectateHandler OnActivitySpectate;

            public ActivityJoinRequestHandler OnActivityJoinRequest;

            public ActivityInviteHandler OnActivityInvite;
        }

        [StructLayout(LayoutKind.Sequential)]
        public partial struct FFIMethods
        {
            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate Result RegisterCommandMethod(nint methodsPtr, [MarshalAs(UnmanagedType.LPStr)] string command);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate Result RegisterSteamMethod(nint methodsPtr, uint steamId);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void UpdateActivityCallback(nint ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void UpdateActivityMethod(nint methodsPtr, ref Activity activity, nint callbackData, UpdateActivityCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void ClearActivityCallback(nint ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void ClearActivityMethod(nint methodsPtr, nint callbackData, ClearActivityCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void SendRequestReplyCallback(nint ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void SendRequestReplyMethod(nint methodsPtr, long userId, ActivityJoinRequestReply reply, nint callbackData, SendRequestReplyCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void SendInviteCallback(nint ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void SendInviteMethod(nint methodsPtr, long userId, ActivityActionType type, [MarshalAs(UnmanagedType.LPStr)] string content, nint callbackData, SendInviteCallback callback);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void AcceptInviteCallback(nint ptr, Result result);

            [UnmanagedFunctionPointer(CallingConvention.Winapi)]
            public delegate void AcceptInviteMethod(nint methodsPtr, long userId, nint callbackData, AcceptInviteCallback callback);

            public RegisterCommandMethod RegisterCommand;

            public RegisterSteamMethod RegisterSteam;

            public UpdateActivityMethod UpdateActivity;

            public ClearActivityMethod ClearActivity;

            public SendRequestReplyMethod SendRequestReply;

            public SendInviteMethod SendInvite;

            public AcceptInviteMethod AcceptInvite;
        }
    }
}