using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public Tile tile;
    public GameMap map;
    public MeshRenderer meshRendererr;

    public Material matWater;
    public Material matGrass;
    public Material matEarth;

    public void UpdateTerrain()
    {
        switch (tile.TerrainType)
        {
            case Tile.TERRAIN_TYPE.WATER:
                meshRendererr.material = matWater;

                break;
            case Tile.TERRAIN_TYPE.GRASS:
                meshRendererr.material = matGrass;

                break;
            case Tile.TERRAIN_TYPE.EARTH:
                meshRendererr.material = matEarth;

                break;

        }
    }
}
