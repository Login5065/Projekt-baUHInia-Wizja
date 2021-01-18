using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelect : MonoBehaviour
{
    public GameObject adminPanel;
    public GameObject mapLocalNameIF;
    public GameObject mapServerNameIF;
    public GameObject errorText;

    void Start()
    {
        if (MapManager.Instance != null)
        {
            UpdateMapManagerReferences();

        }
    }

    public void UpdateMapManagerReferences()
    {
        MapManager.Instance.adminPanel = adminPanel;
        MapManager.Instance.mapLocalNameIF = mapLocalNameIF;
        MapManager.Instance.mapServerNameIF = mapServerNameIF;
        MapManager.Instance.errorText = errorText;
    }

    public void LoadLocalMapBtn()
    {
        MapManager.Instance.LoadLocalMap();
    }

    public void LoadServerMapBtn()
    {
        MapManager.Instance.GetServerMapList();
    }

    public void CreateMapBtn()
    {
        MapManager.Instance.CreateNewMap();
    }

}
