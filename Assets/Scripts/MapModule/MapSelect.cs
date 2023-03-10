using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{
    public GameObject adminPanel;
    public GameObject adminOnlinePanel;
    public GameObject serverPanel;
    public GameObject mapLocalNameIF;
    public GameObject mapServerNameIF;
    public GameObject mapSolutionNameIF;
    public GameObject errorText;
    public TMP_Dropdown objectDropdown;
    public TMP_Dropdown objectDropdownLocal;
    public TMP_Dropdown objectDropdownSolution;


    public List<string> localMaps;

    void Start()
    {
        ListLocalMaps();
        if (MapManager.Instance != null)
        {
            UpdateMapManagerReferences();

        }

        MapManager.Instance.currentGameData = null;
        if (!UserInfo.Instance.isAdmin)
        {
            adminPanel.SetActive(false);
        }
        else
        {
            adminPanel.SetActive(true);

        }

        if (!UserInfo.Instance.isOnline)
        {
            serverPanel.SetActive(false);
            adminOnlinePanel.SetActive(false);
        }
        else
        {
            serverPanel.SetActive(true);
            adminOnlinePanel.SetActive(true);
        }
    }

    public void UpdateMapManagerReferences()
    {
        MapManager.Instance.mapSelect = this;
        MapManager.Instance.adminPanel = adminPanel;
        MapManager.Instance.mapLocalNameIF = mapLocalNameIF;
        MapManager.Instance.mapServerNameIF = mapServerNameIF;
        MapManager.Instance.mapSolutionNameIF = mapSolutionNameIF;
        MapManager.Instance.errorText = errorText;
        MapManager.Instance.objectDropdown = objectDropdown;
        MapManager.Instance.objectDropdownLocal = objectDropdownLocal;
        MapManager.Instance.objectDropdownSolution = objectDropdownSolution;

        MapManager.Instance.mapEntryToLoad = null;
        if (UserInfo.Instance.isOnline)
            MapManager.Instance.GetServerMapList();
    }

    public void LoadLocalMapBtn()
    {
        MapManager.Instance.LoadLocalMap();
    }

    public void LoadServerMapBtn()
    {
        if (UserInfo.Instance.isOnline)
        {
            MapManager.Instance.LoadServerMap();

        }
        else
        {

        }
    }

    public void LoadServerSolutionBtn()
    {
        if (UserInfo.Instance.isOnline)
        {
            MapManager.Instance.LoadSolutionMap();

        }
        else
        {

        }

    }

    public void CreateMapBtn()
    {
        MapManager.Instance.CreateNewMap();
    }

    public void BackToMenuBtn()
    {
        Destroy(MapManager.Instance.gameObject);
        Destroy(UserInfo.Instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void ListLocalMaps()
    {
        string path = Application.persistentDataPath + "/LocalMaps/";
        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            string tmp = file.Name.Remove(file.Name.Length - 5);
            localMaps.Add(tmp);
            Debug.Log(tmp);
        }
        MapManager.Instance.PopulateDropdown(objectDropdownLocal, localMaps);
        SelectLocalEntry(0);
    }

    public void SelectLocalEntry(int option)
    {
        if (localMaps.Count == 0)
        {
            Debug.Log("NO LOCAL MAPS");
            return;
        }
        if (MapManager.Instance != null)
        {
            //string tmp = localMaps[option].Remove(localMaps[option].Length - 5);
            MapManager.Instance.localMapName = localMaps[option];
            Debug.Log("Dropdown option number:" + option + "  that is " + localMaps[option]);
        }
    }

    public void SelectServerEntry(int option)
    {
        if (MapManager.Instance != null)
        {
            MapManager.Instance.mapEntryToLoad = MapManager.Instance.mapEntries[option];
            Debug.Log("Dropdown option number:" + option + "  that is " + MapManager.Instance.mapEntries[option].mapName);

        }
    }

    public void SelectSolutionEntry(int option)
    {
        if (MapManager.Instance != null)
        {
            MapManager.Instance.mapEntryToLoadSolution = MapManager.Instance.mapEntriesSolutions[option];
            Debug.Log("Dropdown option number:" + option + "  that is " + MapManager.Instance.mapEntriesSolutions[option].mapName);

        }
    }

}
