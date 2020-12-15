using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MapData
{
    public string mapName;
    public double budget;
    public int columnsNumber;
    public int rowsNumber;
    public Tile[] tiles;
    public MapHeat mapHeat;
}
