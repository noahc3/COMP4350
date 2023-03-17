public static class Endpoints {
    // Auth
    public const string V1_AUTH_REGISTER = "/v1/auth/register";
    public const string V1_AUTH_LOGIN = "/v1/auth/login";
    public const string V1_AUTH_CHECKSESSION = "/v1/auth/checksession";

    // User
    public const string V1_USER_PROFILE = "/v1/user/profile";
    public const string V1_USER_LOGOUT = "/v1/user/logout";

    // Spool
    public const string V1_SPOOL_GET = "/v1/spool/{0}";
    public const string V1_SPOOL_GET_THREADS = "/v1/spool/threads/{0}";
    public const string V1_SPOOL_GET_THREADS_FILTERED = "/v1/spool/threads/{0}/{1}";
    public const string V1_SPOOL_GET_ALL = "/v1/spool/all";
    public const string V1_SPOOL_CREATE = "/v1/spool/create";
    public const string V1_SPOOL_DELETE = "/v1/spool/delete/{0}";
    public const string V1_SPOOL_JOINED = "/v1/spool/joined/{0}";
    public const string V1_SPOOL_MODS = "/v1/spool/mods/{0}";
    public const string V1_SPOOL_ADD_MOD = "/v1/spool/mods/add/{0}/{1}";
    public const string V1_SPOOL_REMOVE_MOD = "/v1/spool/mods/remove/{0}/{1}";
    public const string V1_SPOOL_CHANGE_OWNER = "/v1/spool/change/{0}/{1}";
    public const string V1_SPOOL_SAVE_RULES = "/v1/spool/save/{0}";

    // UserSettings
    public const string V1_USERSETTINGS_JOIN = "/v1/userSettings/join/{0}";
    public const string V1_USERSETTINGS_REMOVE = "/v1/userSettings/remove/{0}";
    public const string V1_USERSETTINGS_CHECK = "/v1/userSettings/check/{0}";

    // Thread
    public const string V1_THREAD_GET = "/v1/thread/{0}";
    public const string V1_THREAD_GET_ALL = "/v1/thread/all";
    public const string V1_THREAD_GET_ALL_FILTERED = "/v1/thread/all/{0}";
    public const string V1_THREAD_CREATE = "/v1/thread/create";
    public const string V1_THREAD_EDIT = "/v1/thread/edit";
    public const string V1_THREAD_STITCH = "/v1/thread/stitch";
    public const string V1_THREAD_RIP = "/v1/thread/rip";
    public const string V1_THREAD_DELETE = "/v1/thread/{0}";

    // Comment
    public const string V1_COMMENT_BASE = "/v1/comments/{0}";
    public const string V1_COMMENT_CREATE = "/v1/comments/{0}/{1}";
    public const string V1_COMMENT_EXPAND = "/v1/comments/{0}/expand/{1}";
    public const string V1_COMMENT_OLDER = "/v1/comments/{0}/older/{1}";
    public const string V1_COMMENT_NEWER = "/v1/comments/{0}/newer/{1}";
    public const string V1_COMMENT_EDIT = "/v1/comments/{0}/edit";
    public const string V1_COMMENT_DELETE = "/v1/comments/{0}/{1}";
}