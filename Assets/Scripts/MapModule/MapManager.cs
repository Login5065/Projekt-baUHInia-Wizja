using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public GameObject adminPanel;
    public GameObject mapLocalNameIF;
    public GameObject mapServerNameIF;
    public GameObject errorText;

    public GameData currentGameData;
    MapEntry mapEntryToLoad;
    bool mapEntriesLoaded;
    private void Awake()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/LocalMaps/"))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(Application.persistentDataPath + "/LocalMaps/");

        }

        if (Instance == false)
        {
            Instance = this;

        }
        else
        {
            Destroy(this.gameObject);
        }

        if (UserInfo.Instance == null)
            Debug.LogError("No UserInfo currently exists!");


        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        currentGameData = null;
        if (!UserInfo.Instance.isAdmin)
        {
            adminPanel.SetActive(false);
        }
        else
        {
            adminPanel.SetActive(true);

        }
    }

    public void CreateNewMap()
    {
        currentGameData = null;
        SceneManager.LoadScene("MapEditorTEST");
    }

    public void SaveServerMap(GameData gameData)
    {
        StopAllCoroutines();
        StartCoroutine(UploadServerGameDataRoutine(gameData));
    }

    public void LoadGameData(GameData _gameData)
    {
        if (_gameData == null)
        {
            errorText.GetComponent<Text>().text = "Nie znaleziono takiej mapy!";
            Debug.LogError("No GameData to load!");
            return;
        }
        else
        {
            currentGameData = _gameData;
            string jsonMapData = JsonUtility.ToJson(currentGameData, true);
            File.WriteAllText((Application.persistentDataPath+ "/LocalMaps/" + currentGameData.mapName + ".json"), jsonMapData);
            SceneManager.LoadScene("MapEditorTEST");
        }

    }

    public void LoadLocalMap()
    {
        bool mapFound = false;
        currentGameData = LoadLocal(mapLocalNameIF.GetComponent<InputField>().text);

        if (currentGameData != null)
            SceneManager.LoadScene("MapEditorTEST");
    }

    public void LoadServerMap(MapEntry[] entries)
    {
        bool mapFound = false;

        //GetServerMapList();
        if (entries.Length > 0)
        {
            Debug.Log("List of maps if bigger then 0!");
            string mapName = mapServerNameIF.GetComponent<InputField>().text;
            foreach (MapEntry entry in entries)
            {
                if (entry.mapName == mapName)
                {
                    mapEntryToLoad = entry;
                    Debug.Log("FoundMap!");
                    mapFound = true;
                }


                if (mapFound == true)
                {
                    StopAllCoroutines();
                    StartCoroutine(GetServerMapData(LoadGameData));

                }
            }
        }
        //SceneManager.LoadScene("MapEditorTEST");
    }

    public void GetServerMapList()
    {
        StopAllCoroutines();
        StartCoroutine(GetServerMapListRoutine(LoadServerMap));
    }

    IEnumerator UploadServerGameDataRoutine(GameData gameData)
    {

        int requestID;
        MapSaveRequest mapSaveRequest = new MapSaveRequest();
        mapSaveRequest.mapGameData = gameData;
        mapSaveRequest.accToken = UserInfo.accessToken;
        mapSaveRequest.isSolution = !UserInfo.Instance.isAdmin;


        string json = JsonUtility.ToJson(mapSaveRequest);

        if (UserInfo.Instance.isAdmin)
        {
            //request admin not implemented yet?
           // requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_UPLOAD_ADMIN);
            requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_UPLOAD_USER);
        }
        else
        {
            requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_UPLOAD_USER);
        }

        Response response = null;
        while (response == null)
        {
            response = RequestMan.GetRequest(requestID);
            yield return new WaitForSeconds(0.5f);
        }

        //OtherError(response);

        if (response.status == Response.ResponseType.RES_ERROR_SAVE_FAILED)
        {
            //errorText.GetComponent<Text>().text = "RES_ERROR_SAVE_FAILED";
        }
        if (response.status == Response.ResponseType.RES_ERROR_USER_NOT_FOUND)
        {
            //errorText.GetComponent<Text>().text = "Nie znaleziona takiego użytkownika!";
        }
        if (response.status == Response.ResponseType.RES_ERROR_PASSWORD_INVALID)
        {
           // errorText.GetComponent<Text>().text = "Błędne hasło!";
        }
        if (response.status == Response.ResponseType.RES_SUCCESS)
        {
            MapSaveResponse mapListResponse = JsonUtility.FromJson<MapSaveResponse>(response.jsonResponse);
            Debug.Log("Map saved to server");

        }
        else
        {
           // errorText.GetComponent<Text>().text = "Nie przewidziana odpowiedź serwera!";
        }

    }

    IEnumerator GetServerMapListRoutine(Action<MapEntry[]> callback)
    {

        int requestID;
        MapListRequest mapListRequest = new MapListRequest();
        mapListRequest.isSolution = false;

        string json = JsonUtility.ToJson(mapListRequest);
        requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_GETLIST); ;

        Response response = null;
        while (response == null)
        {
            response = RequestMan.GetRequest(requestID);
            yield return new WaitForSeconds(0.5f);
        }

        //OtherError(response);

        if (response.status == Response.ResponseType.RES_ERROR_SAVE_FAILED)
        {
           // errorText.GetComponent<Text>().text = "RES_ERROR_SAVE_FAILED";
        }
        if (response.status == Response.ResponseType.RES_ERROR_USER_NOT_FOUND)
        {
            //errorText.GetComponent<Text>().text = "Nie znaleziona takiego użytkownika!";
        }
        if (response.status == Response.ResponseType.RES_ERROR_PASSWORD_INVALID)
        {
           // errorText.GetComponent<Text>().text = "Błędne hasło!";
        }
        if (response.status == Response.ResponseType.RES_SUCCESS)
        {
            MapListResponse mapListResponse = JsonUtility.FromJson<MapListResponse>(response.jsonResponse);
            Debug.Log("Map list downloaded from server");
            callback(mapListResponse.entries);

        }
        else
        {
            callback(null);
           // errorText.GetComponent<Text>().text = "Nie przewidziana odpowiedź serwera!";
            //TurnOnButtons();
        }

    }

    IEnumerator GetServerMapData(Action<GameData> callback)
    {

        int requestID;
        MapLoadRequest mapLoadRequest = new MapLoadRequest();
        mapLoadRequest.isSolution = false;
        mapLoadRequest.UUID = mapEntryToLoad.UUID;

        string json = JsonUtility.ToJson(mapLoadRequest);
        requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_MAP_DOWNLOAD_USER_NEW);

        Response response = null;
        while (response == null)
        {
            response = RequestMan.GetRequest(requestID);
            yield return new WaitForSeconds(0.5f);
        }

        //OtherError(response);

        if (response.status == Response.ResponseType.RES_ERROR_SAVE_FAILED)
        {
           // errorText.GetComponent<Text>().text = "RES_ERROR_SAVE_FAILED";
        }
        if (response.status == Response.ResponseType.RES_ERROR_USER_NOT_FOUND)
        {
           // errorText.GetComponent<Text>().text = "Nie znaleziona takiego użytkownika!";
        }
        if (response.status == Response.ResponseType.RES_ERROR_PASSWORD_INVALID)
        {
           // errorText.GetComponent<Text>().text = "Błędne hasło!";
        }
        if (response.status == Response.ResponseType.RES_SUCCESS)
        {
            MapLoadResponse mapGameDataResponse = JsonUtility.FromJson<MapLoadResponse>(response.jsonResponse);
            Debug.Log("Game data downloaded from server");
            callback(mapGameDataResponse.mapGameData);

        }
        else
        {
            callback(null);
            //errorText.GetComponent<Text>().text = "Nie przewidziana odpowiedź serwera!";
            //TurnOnButtons();
        }

    }

    public static GameData UpdateGameDataInfo(GameData gameData)
    {
        gameData.author = UserInfo.Instance.login;
        gameData.date = System.DateTime.Now.ToString();

        return gameData;
    }

    public static GameData GenerateNewGameData()
    {
        GameData gameData = new GameData();
        gameData.UUID = System.Guid.NewGuid().ToString();
        gameData.author = UserInfo.Instance.login;
        gameData.date = System.DateTime.Now.ToString();
        gameData.allNeededData = new AllNeededData();
        gameData.allNeededData.mapData = new MapData();

        return gameData;
    }


    /// <summary>
    /// Saves passed mapData to folder as JSON.
    /// </summary>
    /// <param name="mapData">MapData to save</param>
    public static void SaveLocal(GameData gameData)
    {
        gameData = UpdateGameDataInfo(gameData);
        string jsonMapData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText((Application.persistentDataPath + "/LocalMaps/" + gameData.mapName + ".json"), jsonMapData);
    }

    /// <summary>
    /// Returns MapData with provided name from local folder.
    /// </summary>
    /// <param name="mapName">Name of map to load</param>
    /// <returns>MapData of map with passed name</returns>
    public static GameData LoadLocal(string mapName)
    {
        string savePath = Application.persistentDataPath + "/LocalMaps/" + mapName + ".json";
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        else
        {
            Debug.LogWarning("Saved map doesn't exist.");
            return null;
        }
    }
}
