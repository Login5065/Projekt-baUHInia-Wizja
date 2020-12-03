using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Request {
    public enum RequestType {
        REQ_TEST,
        REQ_REGISTER,
        REQ_LOGIN,
        REQ_LOGOUT,
        REQ_ACCESS_USER,
        REQ_REFRESH_USER,
        REQ_MAP_DOWNLOAD_USER_NEW,
        REQ_MAP_DOWNLOAD_USER_EDIT,
        REQ_MAP_DOWNLOAD_USER_CHECK,
        REQ_MAP_UPLOAD_USER,
        REQ_MAP_UPLOAD_ADMIN,
        REQ_MAP_CHECK,
        REQ_MAP_GETLIST
    }

    private string jsonRequest;
    private RequestType type;
    private Response response;

    private UnityWebRequest unityReq;
    private UnityWebRequestAsyncOperation unityRes;

    private int id;

    private float initTime;
    private float lastTime;

    static private int nextId = 0;

    public Request(string jsonString, RequestType rType) {
        lastTime = initTime = Time.realtimeSinceStartup;

        jsonRequest = jsonString;
        type = rType;

        switch (type) {
            case RequestType.REQ_TEST:
                RequestTest();
                break;
            case RequestType.REQ_REGISTER:
                RequestRegister();
                break;
            case RequestType.REQ_LOGIN:
                RequestLogin();
                break;
            case RequestType.REQ_LOGOUT:
                RequestLogout();
                break;
            case RequestType.REQ_ACCESS_USER:
                RequestAccessVerification();
                break;
            case RequestType.REQ_REFRESH_USER:
                RequestRefreshToken();
                break;
        }
        id = GetNextId();
    }

    public int GetId() {
        return id;
    }

    public bool GetStatus() {
        return unityRes.isDone;
    }

    public float GetTime() {
        return Time.realtimeSinceStartup - initTime;
    }

    public float GetTimeSinceLastCall() {
        float newTime = Time.realtimeSinceStartup;
        float diff = newTime - lastTime;
        lastTime = newTime;
        return diff;
    }

    public Response GetResponse() {
        if (GetStatus()) {
            response = new Response();
            response.jsonResponse = unityReq.downloadHandler.text;
            response.status = Response.ResponseType.RES_ERROR_GENERAL;

            try {
                EmptyReply replyReader = JsonUtility.FromJson<EmptyReply>(response.jsonResponse);

                if (replyReader == null) {
                    Debug.Log(Messages.preRQ + "Response encountered general error.");
                    response.status = Response.ResponseType.RES_ERROR_GENERAL;
                } else if (!replyReader.status) {
                    Debug.Log(Messages.preRQ + "Response encountered specific error.");
                    switch (replyReader.msg) {
                        case "RES_ERROR_SAVE_FAILED":
                            response.status = Response.ResponseType.RES_ERROR_SAVE_FAILED;
                            break;
                        case "RES_SUCCESS":
                            response.status = Response.ResponseType.RES_SUCCESS;
                            break;
                        case "RES_ERROR_INSUFFICIENT":
                            response.status = Response.ResponseType.RES_ERROR_INSUFFICIENT;
                            break;
                        case "RES_ERROR_GENERAL":
                            response.status = Response.ResponseType.RES_ERROR_GENERAL;
                            break;
                        case "RES_ERROR_PASSWORD_INVALID":
                            response.status = Response.ResponseType.RES_ERROR_PASSWORD_INVALID;
                            break;
                        case "RES_ERROR_USER_NOT_FOUND":
                            response.status = Response.ResponseType.RES_ERROR_USER_NOT_FOUND;
                            break;
                        case "RES_ERROR_NO_TOKEN":
                            response.status = Response.ResponseType.RES_ERROR_NO_TOKEN;
                            break;
                        case "RES_ERROR_BAD_TOKEN":
                            response.status = Response.ResponseType.RES_ERROR_BAD_TOKEN;
                            break;
                    }
                } else {
                    Debug.Log(Messages.preRQ + "Successful response.");
                    response.status = Response.ResponseType.RES_SUCCESS;
                }
            } catch (System.ArgumentException) {
                Debug.Log(Messages.preRQ + "Not a JSON response.");
                response.status = Response.ResponseType.RES_SUCCESS;
            }

            return response;
        } else {
            return null;
        }
    }

    static private int GetNextId() {
        return nextId++;
    }

    private void CreateGetRequest(string route) {
        unityReq = UnityWebRequest.Get(Config.ServerAddress + route);
        unityReq.certificateHandler = new Certificate();
        unityReq.timeout = Config.RequestTimeout;
    }

    private void CreatePostRequest(string route, string json) {
        unityReq = new UnityWebRequest(Config.ServerAddress + route, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        unityReq.uploadHandler = new UploadHandlerRaw(bodyRaw);
        unityReq.downloadHandler = new DownloadHandlerBuffer();
        unityReq.SetRequestHeader("Content-Type", "application/json");
        unityReq.certificateHandler = new Certificate();
        unityReq.timeout = Config.RequestTimeout;
    }

    private void RequestTest() {
        CreateGetRequest("/");
        unityRes = unityReq.SendWebRequest();
    }

    private void RequestRegister() {
        CreatePostRequest(Config.ServerApiRegister, jsonRequest);
        unityRes = unityReq.SendWebRequest();
    }

    private void RequestLogin() {
        CreatePostRequest(Config.ServerApiLogin, jsonRequest);
        unityRes = unityReq.SendWebRequest();
    }

    private void RequestLogout() {
        CreatePostRequest(Config.ServerApiLogout, jsonRequest);
        unityRes = unityReq.SendWebRequest();
    }

    private void RequestAccessVerification() {
        CreatePostRequest(Config.ServerApiAccessVerify, jsonRequest);
        unityRes = unityReq.SendWebRequest();
    }

    private void RequestRefreshToken() {
        CreatePostRequest(Config.ServerApiRefresh, jsonRequest);
        unityRes = unityReq.SendWebRequest();
    }
}
