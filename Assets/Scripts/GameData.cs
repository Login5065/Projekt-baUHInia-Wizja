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

    public void AddHeatData(HeatData _heatData)
    {
        allNeededData.heatData = _heatData;
    }

    public int getBudget()
    {
        return allNeededData.mapData.budget;
    }

}

[System.Serializable]
public struct AllNeededData
{
    public AllNeededData(MapData _mapData, Buildings _buildings, HeatData _heatData)
    {
        mapData = _mapData;
        buildings = _buildings;
        heatData = _heatData;
    }

    public MapData mapData;
    public Buildings buildings;
    public HeatData heatData;
    //public BuildingData buildingData;
    //public MapHeatData mapHeatData;

}
