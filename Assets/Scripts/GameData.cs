using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string mapName;
    public string author;
    public string date;
    //public double budget;

    public string UUID;


    public AllNeededData allNeededData;
}

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
