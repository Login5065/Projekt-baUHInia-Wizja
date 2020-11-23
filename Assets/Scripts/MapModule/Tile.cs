using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Tile
{
    public Tile(int x, int y)
    {
        this.X = x;
        this.Y = y;
        TerrainType = TERRAIN_TYPE.GRASS;
        canPlaceObjects = true;
        temperature = 20f;
        //objects = new List<BUILDING_TYPE>();
    }
    public int X; // Column
    public int Y;   // Row
    public enum TERRAIN_TYPE { GRASS, BARREN, WATER, EARTH }
    public TERRAIN_TYPE TerrainType;
    public float temperature;

    public bool canPlaceObjects;
    //list of builidingObjects on tile
    // public List<BUILDING_TYPE> objects;



}
