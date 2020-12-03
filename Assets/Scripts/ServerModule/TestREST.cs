using UnityEngine;

public class TestREST : MonoBehaviour {
    private int counter = 0;
    private int requestId;
    private bool finished = false;
    private string accessToken = "";
    private string refreshToken = "";

    private void Start() {
        Config.ModuleVerbose = true;
    }

    void Update() {
        if (finished) return;

        if (Time.deltaTime > 0.050 && counter > 30) {
            Debug.LogWarning(Messages.preTR + "Frame time over 50 ms! : " + Time.deltaTime * 1000 + " ms");
        }

        if (counter == 50) {
            Debug.Log(Messages.preTR + "Sending basic test GET request");
            requestId = RequestMan.SendRequest("", Request.RequestType.REQ_TEST);
            if (RequestMan.CheckRequest(requestId)) {
                Debug.LogWarning(Messages.preTR + "Immediate request check true (surprising)");
            } else {
                Debug.Log(Messages.preTR + "Immediate request check returns false (this is expected)");
            }
        } else if (counter == 60) {
            if (RequestMan.CheckRequest(requestId)) {
                Debug.Log(Messages.preTR + "Delayed request check returns true (this is expected)");
            } else {
                counter -= 10;
                Debug.Log(Messages.preTR + "Delayed request check returns false");
            }
        } else if (counter == 70) {
            Debug.Log(Messages.preTR + "Request should've arived by now. Get response!");
            Response res = RequestMan.GetRequest(requestId);
            Debug.Log(Messages.preTR + "res: " + res.jsonResponse);
            Debug.Log(Messages.preTR + "resType: " + res.status);

            Debug.Log(Messages.preTR + "It should not possible to receive response more than once. Check if it was deleted as it should.");
            res = RequestMan.GetRequest(requestId);
            if (res == null) {
                Debug.Log(Messages.preTR + "Response is null. Just as expected.");
            } else {
                Debug.LogError(Messages.preTR + "Response is not null... YOU WERE NOT SUPPOSED TO DO THAT.");
            }
        } else if (counter == 80) {
            Debug.Log(Messages.preTR + "Testing register route");
            RegisterRequest temp = new RegisterRequest();
            temp.username = "testuser";
            temp.password = "testpass";
            temp.email = "test@test.test";
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_REGISTER);
        } else if (counter == 90) {
            Debug.Log(Messages.preTR + "Get register response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        } else if (counter == 100) {
            Debug.Log(Messages.preTR + "Testing login route");
            LoginRequest temp = new LoginRequest();
            temp.username = "testuser";
            temp.password = "testpass";
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGIN);
        } else if (counter == 110) {
            Debug.Log(Messages.preTR + "Get login response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
                LoginReply rep = JsonUtility.FromJson<LoginReply>(res.jsonResponse);
                accessToken = rep.accToken;
                refreshToken = rep.refToken;
            }
        } else if (counter == 120) {
            Debug.Log(Messages.preTR + "Testing access verification");
            VerifyRequest temp = new VerifyRequest();
            temp.accToken = accessToken;
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_ACCESS_USER);
        } else if (counter == 130) {
            Debug.Log(Messages.preTR + "Get access verification response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
                VerifyReply rep = JsonUtility.FromJson<VerifyReply>(res.jsonResponse);
                Debug.Log(Messages.preTR + rep.username + " | is privileged?: " + rep.privilege);
            }
        } else if (counter == 140) {
            Debug.Log(Messages.preTR + "Testing refresh token");
            RefreshRequest temp = new RefreshRequest();
            temp.refToken = refreshToken;
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_REFRESH_USER);
        } else if (counter == 150) {
            Debug.Log(Messages.preTR + "Get refresh token response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
                LoginReply rep = JsonUtility.FromJson<LoginReply>(res.jsonResponse);
                accessToken = rep.accToken;
                refreshToken = rep.refToken;
            }
        } else if (counter == 200) {
            Debug.Log(Messages.preTR + "Testing logout route");
            LogoutRequest temp = new LogoutRequest();
            temp.refToken = refreshToken;
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGOUT);
        } else if (counter == 210) {
            Debug.Log(Messages.preTR + "Get logout response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
                finished = true;
            }
        }

        counter++;
    }
}
