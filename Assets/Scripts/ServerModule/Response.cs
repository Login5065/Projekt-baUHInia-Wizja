public class Response {
    public enum ResponseType {
        RES_SUCCESS,
        RES_ERROR_GENERAL,
        RES_ERROR_NETWORK,
        RES_ERROR_HTTP,
        RES_ERROR_INSUFFICIENT,
        RES_ERROR_SAVE_FAILED,
        RES_ERROR_PASSWORD_INVALID, 
        RES_ERROR_USER_NOT_FOUND,
        RES_ERROR_NO_TOKEN,
        RES_ERROR_BAD_TOKEN
    }

    public string jsonResponse;
    public ResponseType status;
}
