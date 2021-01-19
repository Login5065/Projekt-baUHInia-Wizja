using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string mapName;
    public string author;
    public string date;
    public int budget;

    public string UUID;


    public AllNeededData allNeededData;


    public void AddMapData(MapData _mapData)
    {
        allNeededData.mapData = _mapData;
    }

    public int getBudget()
    {
        return allNeededData.mapData.budget;
    }

}

[System.Serializable]
public struct AllNeededData
{
    public AllNeededData(MapData _mapData)
    {
        mapData = _mapData;
    }

    public MapData mapData;
    //public BuildingData buildingData;
    //public MapHeatData mapHeatData;

}
