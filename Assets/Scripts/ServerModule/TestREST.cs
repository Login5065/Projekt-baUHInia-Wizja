using System.Linq;
using UnityEngine;

public class TestREST : MonoBehaviour {
    private int counter = 0;
    private int requestId;
    private bool finished = false;
    private string accessToken = "";
    private string refreshToken = "";
    private string chosenUUID = "";

    private GameData data;

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
            }
            else {
                Debug.Log(Messages.preTR + "Immediate request check returns false (this is expected)");
            }
        }
        else if (counter == 60) {
            if (RequestMan.CheckRequest(requestId)) {
                Debug.Log(Messages.preTR + "Delayed request check returns true (this is expected)");
            }
            else {
                counter -= 10;
                Debug.Log(Messages.preTR + "Delayed request check returns false");
            }
        }
        else if (counter == 70) {
            Debug.Log(Messages.preTR + "Request should've arived by now. Get response!");
            Response res = RequestMan.GetRequest(requestId);
            Debug.Log(Messages.preTR + "res: " + res.jsonResponse);
            Debug.Log(Messages.preTR + "resType: " + res.status);

            Debug.Log(Messages.preTR +
                      "It should not possible to receive response more than once. Check if it was deleted as it should.");
            res = RequestMan.GetRequest(requestId);
            if (res == null) {
                Debug.Log(Messages.preTR + "Response is null. Just as expected.");
            }
            else {
                Debug.LogError(Messages.preTR + "Response is not null... YOU WERE NOT SUPPOSED TO DO THAT.");
            }
        }
        else if (counter == 80) {
            Debug.Log(Messages.preTR + "Testing register route");
            RegisterRequest temp = new RegisterRequest();
            temp.username = "testuser";
            temp.password = "testpass";
            temp.email = "test@test.test";
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_REGISTER);
        }
        else if (counter == 90) {
            Debug.Log(Messages.preTR + "Get register response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 100) {
            Debug.Log(Messages.preTR + "Testing login route");
            LoginRequest temp = new LoginRequest();
            temp.username = "testuser";
            temp.password = "testpass";
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGIN);
        }
        else if (counter == 110) {
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
        }
        else if (counter == 120) {
            Debug.Log(Messages.preTR + "Testing access verification");
            VerifyRequest temp = new VerifyRequest();
            temp.accToken = accessToken;
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_ACCESS_USER);
        }
        else if (counter == 130) {
            Debug.Log(Messages.preTR + "Get access verification response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
                VerifyReply rep = JsonUtility.FromJson<VerifyReply>(res.jsonResponse);
                Debug.Log(Messages.preTR + rep.username + " | is privileged?: " + rep.privilege);
            }
        }
        else if (counter == 140) {
            Debug.Log(Messages.preTR + "Testing refresh token");
            RefreshRequest temp = new RefreshRequest();
            temp.refToken = refreshToken;
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_REFRESH_USER);
        }
        else if (counter == 150) {
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
        }
        else if (counter == 200) {
            Debug.Log(Messages.preTR + "Testing logout route");
            LogoutRequest temp = new LogoutRequest();
            temp.refToken = refreshToken;
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGOUT);
        }
        else if (counter == 210) {
            Debug.Log(Messages.preTR + "Get logout response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 220) {
            Debug.Log(Messages.preTR + "Try sending the map - login");
            LoginRequest temp = new LoginRequest();
            temp.username = "testuser";
            temp.password = "testpass";
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGIN);
        }
        else if (counter == 230) {
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
        }
        else if (counter == 240) {
            Debug.Log(Messages.preTR + "Try sending the map - map save request");
            MapSaveRequest temp = new MapSaveRequest();

            temp.isSolution = true;
            temp.accToken = accessToken;
            temp.mapGameData.author = "testuser";
            temp.mapGameData.date = System.DateTime.Now.ToString();
            temp.mapGameData.mapName = "testmap";
            temp.mapGameData.UUID = System.Guid.NewGuid().ToString();
            temp.mapGameData.allNeededData.mapData.mapName =
                "oh so much data i don't know what it can be oh stop it oni-chan";

            data = temp.mapGameData;

            chosenUUID = temp.mapGameData.UUID;

            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_UPLOAD_USER);
        }
        else if (counter == 250) {
            Debug.Log(Messages.preTR + "Try sending the map - get map save response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 260) {
            Debug.Log(Messages.preTR + "List maps");
            MapListRequest temp = new MapListRequest();
            temp.isSolution = true;

            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_GETLIST);
        }
        else if (counter == 270) {
            Debug.Log(Messages.preTR + "Try getting the list");
            Response res = RequestMan.GetRequest(requestId);

            if (res == null) counter -= 10;
            else {
                MapListResponse objRes = JsonUtility.FromJson<MapListResponse>(res.jsonResponse);
                Debug.Log(Messages.preTR + "Received " + objRes.entries.Length + " documents with following UUIDs:");

                foreach (MapEntry entry in objRes.entries) {
                    Debug.Log(Messages.preTR + "UUID: " + entry.UUID);
                }

                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 280) {
            Debug.Log(Messages.preTR + "List one specific map");
            MapListRequest temp = new MapListRequest();
            temp.UUID = chosenUUID;
            temp.isSolution = true;

            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_GETLIST);
        }
        else if (counter == 290) {
            Debug.Log(Messages.preTR + "Try getting the list (should be only one document)");
            Response res = RequestMan.GetRequest(requestId);

            if (res == null) counter -= 10;
            else {
                MapListResponse objRes = JsonUtility.FromJson<MapListResponse>(res.jsonResponse);
                Debug.Log(Messages.preTR + "Received " + objRes.entries.Length + " documents with following UUIDs:");

                foreach (MapEntry entry in objRes.entries) {
                    Debug.Log(Messages.preTR + "UUID: " + entry.UUID);
                }

                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 300) {
            Debug.Log(Messages.preTR + "Load map");
            MapLoadRequest temp = new MapLoadRequest();
            temp.UUID = chosenUUID;
            temp.isSolution = true;

            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_DOWNLOAD_USER_NEW);
        }
        else if (counter == 310) {
            Debug.Log(Messages.preTR + "Try getting this map");
            Response res = RequestMan.GetRequest(requestId);

            if (res == null) counter -= 10;
            else {
                MapLoadResponse map = JsonUtility.FromJson<MapLoadResponse>(res.jsonResponse);

                if (map.mapGameData.allNeededData.mapData.mapName == data.allNeededData.mapData.mapName) {
                    Debug.Log(Messages.preTR + "Data sent is the same as received. All good!");
                }
                else {
                    Debug.LogError(Messages.preTR + "Data sent is NOT the same as received!");
                }

                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 320) {
            Debug.Log(Messages.preTR + "Logout route");
            LogoutRequest temp = new LogoutRequest();
            temp.refToken = refreshToken;
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGOUT);
        }
        else if (counter == 330) {
            Debug.Log(Messages.preTR + "Logout response");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 340) {
            Debug.Log(Messages.preTR + "Test admin privileges - create admin account first");
            RegisterRequest temp = new RegisterRequest();
            temp.username = "testadmin";
            temp.password = "testpass";
            temp.email = "admin@test.test";
            temp.admin = true;
            
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_REGISTER);
        }
        else if (counter == 350) {
            Debug.Log(Messages.preTR + "Check register");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 360) {
            Debug.Log(Messages.preTR + "Login admin");
            LoginRequest temp = new LoginRequest();
            temp.username = "testadmin";
            temp.password = "testpass";
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGIN);
        }
        else if (counter == 370) {
            Debug.Log(Messages.preTR + "Check login");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
                LoginReply rep = JsonUtility.FromJson<LoginReply>(res.jsonResponse);
                accessToken = rep.accToken;
                refreshToken = rep.refToken;
            }
        }
        else if (counter == 380) {
            Debug.Log(Messages.preTR + "Try saving clear map that only admin can save");
            MapSaveRequest temp = new MapSaveRequest();

            temp.isSolution = false;
            temp.accToken = accessToken;
            temp.mapGameData.author = "testadmin";
            temp.mapGameData.date = System.DateTime.Now.ToString();
            temp.mapGameData.mapName = "testmapadmin";
            temp.mapGameData.UUID = System.Guid.NewGuid().ToString();
            temp.mapGameData.allNeededData.mapData.mapName =
                "oh su much admin, so much power, please violate me with your power";

            data = temp.mapGameData;

            chosenUUID = temp.mapGameData.UUID;

            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_UPLOAD_USER);
        }
        else if (counter == 390) {
            Debug.Log(Messages.preTR + "Check");
            Response res = RequestMan.GetRequest(requestId);
            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 400) {
            Debug.Log(Messages.preTR + "List clear maps");
            MapListRequest temp = new MapListRequest();
            temp.isSolution = false;

            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_GETLIST);
        }
        else if (counter == 410) {
            Debug.Log(Messages.preTR + "Get the list");
            Response res = RequestMan.GetRequest(requestId);

            if (res == null) counter -= 10;
            else {
                MapListResponse objRes = JsonUtility.FromJson<MapListResponse>(res.jsonResponse);
                Debug.Log(Messages.preTR + "Received " + objRes.entries.Length + " documents with following UUIDs:");

                foreach (MapEntry entry in objRes.entries) {
                    Debug.Log(Messages.preTR + "UUID: " + entry.UUID);
                }
                
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 420) {
            Debug.Log(Messages.preTR + "Get the admin map");
            MapLoadRequest temp = new MapLoadRequest();
            temp.UUID = chosenUUID;
            temp.isSolution = false;

            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_DOWNLOAD_USER_NEW);
        }
        else if (counter == 430) {
            Debug.Log(Messages.preTR + "Get the map response");
            Response res = RequestMan.GetRequest(requestId);

            if (res == null) counter -= 10;
            else {
                MapLoadResponse map = JsonUtility.FromJson<MapLoadResponse>(res.jsonResponse);

                if (map.mapGameData.allNeededData.mapData.mapName == data.allNeededData.mapData.mapName) {
                    Debug.Log(Messages.preTR + "Data sent is the same as received. All good!");
                }
                else {
                    Debug.LogError(Messages.preTR + "Data sent is NOT the same as received!");
                }
                
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 440) {
            Debug.Log(Messages.preTR + "Overwrite existing map");
            MapSaveRequest temp = new MapSaveRequest();
            temp.accToken = accessToken;
            temp.isSolution = false;
            temp.mapGameData = data;
            temp.mapGameData.allNeededData.mapData.mapName = "this is the new shit";

            data = temp.mapGameData;

            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_UPLOAD_USER);
        }
        else if (counter == 450) {
            Debug.Log(Messages.preTR + "Check");
            Response res = RequestMan.GetRequest(requestId);

            if (res == null) counter -= 10;
            else {
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 460) {
            Debug.Log(Messages.preTR + "Get the overwritten map");
            MapLoadRequest temp = new MapLoadRequest();
            temp.UUID = chosenUUID;
            temp.isSolution = false;

            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_DOWNLOAD_USER_NEW);
        }
        else if (counter == 470) {
            Debug.Log(Messages.preTR + "Get the overwritten map response");
            Response res = RequestMan.GetRequest(requestId);

            if (res == null) counter -= 10;
            else {
                MapLoadResponse map = JsonUtility.FromJson<MapLoadResponse>(res.jsonResponse);

                if (map.mapGameData.allNeededData.mapData.mapName == data.allNeededData.mapData.mapName) {
                    Debug.Log(Messages.preTR + "Data sent is the same as received. All good!");
                }
                else {
                    Debug.LogError(Messages.preTR + "Data sent is NOT the same as received!");
                }
                
                Debug.Log(Messages.preTR + res.jsonResponse);
                Debug.Log(Messages.preTR + res.status);
            }
        }
        else if (counter == 480) {
            Debug.Log(Messages.preTR + "Admin logout");
            LogoutRequest temp = new LogoutRequest();
            temp.refToken = refreshToken;
            string json = JsonUtility.ToJson(temp);
            requestId = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGOUT);
        }
        else if (counter == 490) {
            Debug.Log(Messages.preTR + "Check logout");
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