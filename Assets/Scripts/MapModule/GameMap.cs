using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    public static GameMap Instance;
    public GameObject tilePrefab;
    public GameObject tileHolder;
    public GameData gameData;
    public MapData mapData;
    public string mapName;
    public int columns = 20;
    public int rows = 20;
    public List<TileComponent> tiles;

    private void Awake()
    {
        Instance = this;
    }
    public static Tile GetTileAt(int x, int y)
    {
        if (Instance.mapData.tiles == null)
        {
            Debug.LogError("Tiles array not yet instantiated.");
            return null;
        }
        //need to do it in try catch, becouse some hexes may not have all neighours (map edges)
        try
        {
            return Instance.mapData.tiles[y + (x * Instance.mapData.rowsNumber)];
        }
        catch
        {
            //Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
        }
    }

    public void BuildMap()
    {
        tiles.Clear();
        DestroyMap();
        if (mapData == null)
        {
            mapData = new MapData();
            mapData.tiles = new Tile[columns * rows];
            mapData.columnsNumber = columns;
            mapData.rowsNumber = rows;
            gameData.mapName = mapName;
            //create needed hexes
            for (int column = 0; column < columns; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    Tile tile = new Tile(column, row);
                    mapData.tiles[row + (column * rows)] = tile;
                }
            }
        }
        else
        {
            //map was loaded with existing mapData and hexes
            mapData = gameData.allNeededData.mapData;
            rows = mapData.rowsNumber;
            columns = mapData.columnsNumber;
            mapName = gameData.mapName;
        }

        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                Tile tile = mapData.tiles[row + (column * rows)];
                Vector3 pos = new Vector3(
                    tile.X,
                    0,
                    tile.Y
                );

                GameObject hexGameObject = (GameObject)Instantiate(
                    tilePrefab,
                    pos,
                    Quaternion.identity,
                    tileHolder.transform
                );
                hexGameObject.layer = LayerMask.GetMask("MapTile");

                hexGameObject.name = string.Format("TILE: {0},{1}", column, row);
                TileComponent tileComponent = hexGameObject.GetComponent<TileComponent>();
                tileComponent.tile = tile;
                tileComponent.map = this;
                tileComponent.UpdateTerrain();
                tiles.Add(tileComponent);
            }
        }
        UpdateAllTilesVisuals();
    }
    //mapData should contain hex information already
    public void LoadMap(string name)
    {
        gameData = MapManager.LoadLocal(name);
        if (gameData == null)
        {
            Debug.LogWarning("Map not loaded!");
            return;
        }
        else
        {
            mapData = gameData.allNeededData.mapData;
        }
        BuildMap();
    }

    //mapData should contain hex information already
    public void CreateNewMap()
    {
        mapData = null;
        BuildMap();
    }
    public void DestroyMap()
    {
        for (int i = tileHolder.transform.childCount; i > 0; --i)
            DestroyImmediate(tileHolder.transform.GetChild(0).gameObject);
    }

    public void UpdateAllTilesVisuals()
    {
        foreach (var tile in tiles)
        {
            tile.UpdateTerrain();
        }
    }

    public void SwitchHeatView()
    {
        foreach (var tile in tiles)
        {
            tile.SwitchView();
        }
    }
}
