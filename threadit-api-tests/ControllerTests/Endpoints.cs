public static class Endpoints {
    // Auth
    public const string V1_AUTH_REGISTER = "/v1/auth/register";
    public const string V1_AUTH_LOGIN = "/v1/auth/login";
    public const string V1_AUTH_CHECKSESSION = "/v1/auth/checksession";

    // User
    public const string V1_USER_PROFILE = "/v1/user/profile";

    // Spool
    public const string V1_SPOOL_GET = "/v1/spool/{0}";
    public const string V1_SPOOL_CREATE = "/v1/spool/create";

    // Thread
    public const string V1_THREAD_GET = "/v1/thread/{0}";
    public const string V1_THREAD_CREATE = "/v1/thread/create";

    // Comment
    public const string V1_COMMENT_BASE = "/v1/comments/{0}";
    public const string V1_COMMENT_CREATE = "/v1/comments/{0}/{1}";
    public const string V1_COMMENT_EXPAND = "/v1/comments/{0}/expand/{1}";
    public const string V1_COMMENT_OLDER = "/v1/comments/{0}/older/{1}";
    public const string V1_COMMENT_NEWER = "/v1/comments/{0}/newer/{1}";
    public const string V1_COMMENT_EDIT = "/v1/comments/{0}/edit";
    public const string V1_COMMENT_DELETE = "/v1/comments/{0}/{1}";
}