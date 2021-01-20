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

    public int getBestScore()
    {
        return allNeededData.bestScore;
    }

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

    public List <BuildingObjectData> getBuildingList()
    {
        return allNeededData.buildings.BuildingObjectData;
    }
    public List <BuildingPrefabData> getFinanceList()
    {
        return allNeededData.buildings.tab;
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
        bestScore = 0;
    }

    public MapData mapData;
    public Buildings buildings;
    public HeatData heatData;
    public int bestScore;
    //public BuildingData buildingData;
    //public MapHeatData mapHeatData;

}
