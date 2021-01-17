public static class Config {
    public static bool ModuleVerbose = false;
    public const int RequestTimeout = 60;
    public const string ServerAddress = "https://127.0.0.1:8080";
    public const string ServerApiRegister = "/api/user/register";
    public const string ServerApiLogin = "/api/user/login";
    public const string ServerApiLogout = "/api/user/logout";
    public const string ServerApiAccessVerify = "/api/user/validate";
    public const string ServerApiRefresh = "/api/user/refresh";
    public const string ServerApiMapSave = "/api/user/map/save";
    public const string ServerApiMapLoad = "/api/user/map/load";
    public const string ServerApiMapList = "/api/user/map/list";
}
