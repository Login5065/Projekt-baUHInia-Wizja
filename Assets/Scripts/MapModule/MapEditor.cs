using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour
{
    public static MapEditor Instance;
    public GameObject adminOptions;
    public GameObject userOptions;
    public GameMap map;
    public InputField mapNameInput;
    public InputField rowsNumberInput;
    public InputField columnNumberInput;

    public Tile.TERRAIN_TYPE selectedTerrainType;
    private TileComponent tileHit;

    GameData gameData;

    public bool isEditingTerrain = false;

    private bool userCanEdit = false;
    private void Awake()
    {
        Instance = this;
        rowsNumberInput.contentType = InputField.ContentType.IntegerNumber;
        rowsNumberInput.characterValidation = InputField.CharacterValidation.Integer;
        columnNumberInput.contentType = InputField.ContentType.IntegerNumber;
        columnNumberInput.characterValidation = InputField.CharacterValidation.Integer;

    }

    private void Start()
    {
        if (!UserInfo.Instance.isAdmin)
        {
            adminOptions.SetActive(false);
            userOptions.SetActive(true);
        }
        else
        {
            adminOptions.SetActive(true);
            userOptions.SetActive(false);
        }

        if (MapManager.Instance.currentGameData != null)
        {
            Debug.Log("Found game data on load, loading map!");
            gameData = MapManager.Instance.currentGameData;
            map.LoadMap(gameData.mapName);
        }

    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (isEditingTerrain)
                {
                    EditTile();
                }

            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            isEditingTerrain = false;
        }

    }


    void EditTile()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, LayerMask.GetMask("MapTile")))
        {
            if ((tileHit = rayHit.collider.GetComponent<TileComponent>()) != null)
            {
                tileHit.tile.TerrainType = selectedTerrainType;
                if (selectedTerrainType == Tile.TERRAIN_TYPE.WATER)
                {
                    tileHit.tile.canPlaceObjects = false;
                }
                else
                {
                    tileHit.tile.canPlaceObjects = userCanEdit;
                }
                tileHit.UpdateTerrain();
            }
        }
    }

    public void UpdateGameData()
    {
        map.gameData.AddMapData(map.mapData);
        map.gameData.allNeededData.mapData.mapName = mapNameInput.text;
        map.gameData.mapName = mapNameInput.text;
        map.gameData.author = UserInfo.Instance.login;



    }

    public void LoadMap()
    {
        map.LoadMap(mapNameInput.text);
        gameData = map.gameData;
    }

    public void SaveMap()
    {
        UpdateGameData();
        //map.mapName = mapNameInput.text;
        if (map.gameData == null)
            Debug.Log("map gamedata is null");
        MapManager.SaveLocal(map.gameData);
    }

    public void SaveMapServer()
    {
        UpdateGameData();
        //map.mapName = mapNameInput.text;
        MapManager.Instance.SaveServerMap(map.gameData);
    }

    public void CreateNewMap()
    {
        gameData = MapManager.GenerateNewGameData();
        map.gameData = gameData;
        map.rows = int.Parse(rowsNumberInput.text);
        map.columns = int.Parse(columnNumberInput.text);
        //map.mapName = mapNameInput.text;
        map.CreateNewMap();
    }

    public void SelectTerrain(int option)
    {
        isEditingTerrain = true;
        switch (option)
        {
            case 0:
                selectedTerrainType = Tile.TERRAIN_TYPE.WATER;
                break;
            case 1:
                selectedTerrainType = Tile.TERRAIN_TYPE.EARTH;
                break;
            case 2:
                selectedTerrainType = Tile.TERRAIN_TYPE.GRASS;
                break;

        }
    }

    //for toggle
    public void UserCanEdit(bool canEdit)
    {
        userCanEdit = canEdit;
    }

}
