using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    public InputField tempGlobal;
    public InputField tempMinInput;
    public InputField tempMaxInput;
    public GameObject saveServerButton;
    public Tile.TERRAIN_TYPE selectedTerrainType;
    private TileComponent tileHit;

    GameData gameData;

    public bool isEditingTerrain = false;

    private bool userCanEdit = true;
    private void Awake()
    {
        Instance = this;
        rowsNumberInput.contentType = InputField.ContentType.IntegerNumber;
        rowsNumberInput.characterValidation = InputField.CharacterValidation.Integer;
        columnNumberInput.contentType = InputField.ContentType.IntegerNumber;
        columnNumberInput.characterValidation = InputField.CharacterValidation.Integer;
        tempMinInput.contentType = InputField.ContentType.IntegerNumber;
        tempMinInput.characterValidation = InputField.CharacterValidation.Integer;
        tempMaxInput.contentType = InputField.ContentType.IntegerNumber;
        tempMaxInput.characterValidation = InputField.CharacterValidation.Integer;
        tempGlobal.contentType = InputField.ContentType.IntegerNumber;
        tempGlobal.characterValidation = InputField.CharacterValidation.Integer;

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

        if (!UserInfo.Instance.isOnline)
        {
            saveServerButton.SetActive(false);
        }
        else
        {
            saveServerButton.SetActive(true);
        }

        if (MapManager.Instance.currentGameData != null)
        {
            Debug.Log("Found game data on load, loading map!");
            gameData = MapManager.Instance.currentGameData;
            map.LoadMap(gameData.mapName);
        }
        selectedTerrainType = Tile.TERRAIN_TYPE.WATER;
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
        //if (Input.GetMouseButtonDown(1))
        //{
        //   // isEditingTerrain = false;
        //}

    }


    void EditTile()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, LayerMask.GetMask("MapTile")))
        {
            if ((tileHit = rayHit.collider.GetComponent<TileComponent>()) != null)
            {
                if(tileHit.tile.objectPlaced==false)
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
    }

    public void UpdateGameData()
    {
        map.gameData.AddMapData(map.mapData);
        map.gameData.allNeededData.mapData.mapName = mapNameInput.text;
        map.gameData.mapName = mapNameInput.text;
        map.gameData.author = UserInfo.Instance.login;
        map.gameData.allNeededData.mapData.budget = FindObjectOfType<Budget>().getCurrentAmount();
        map.gameData.allNeededData.buildings = FindObjectOfType<BuildingManagment>().getBuildingData();
        if(MainMenu.isAdmin == true)
        {
            map.gameData.allNeededData.bestScore = FindObjectOfType<Score>().currentScore;
        }
        else
        {
            map.gameData.allNeededData.bestScore = FindObjectOfType<Score>().bestScore;
        }
        map.gameData.allNeededData.heatData = MapHeat.Instance.heatData;

    }

    public void ApplyTempMinMax()
    {
        if (tempMinInput.text == "" || tempMaxInput.text == "")
            return;

        float tempMin = float.Parse(tempMinInput.text);
        float tempMax = float.Parse(tempMaxInput.text);
        float global = float.Parse(tempGlobal.text);

        if (tempMin > tempMax)
        {
            float tmp = tempMax;
            tempMax = tempMin;
            tempMin = tmp;
        }

        if (global > tempMax)
            global = tempMax;

        if (global < tempMin)
            global = tempMin;

        MapHeat.Instance.heatData.tempMin = tempMin;
        MapHeat.Instance.heatData.tempMax = tempMax;
        MapHeat.Instance.heatData.tempMax = global;

        MapHeat.Instance.CalculateTemperature(GameMap.Instance.mapData.tiles);
    }

    public void LoadMap()
    {
        map.LoadMap(mapNameInput.text);
        gameData = map.gameData;
    }

    public void SaveMap()
    {
        if (mapNameInput.text == "" || map.gameData == null)
            return;
        UpdateGameData();
        //map.mapName = mapNameInput.text;
        if (map.gameData == null)
            Debug.Log("map gamedata is null");
        MapManager.SaveLocal(map.gameData);
    }

    public void GoToMapSelect()
    {
        SceneManager.LoadScene("MapSelect");
    }

    public void SaveMapServer()
    {
        if (mapNameInput.text == "" || map.gameData == null)
            return;
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
        //isEditingTerrain = true;
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

    public void EditIsOn(bool isEditing)
    {
        isEditingTerrain = isEditing;
    }

    public void SwitchHeatView()
    {
        MapHeat.Instance.CalculateTemperature(GameMap.Instance.mapData.tiles);
        map.SwitchHeatView();
    }

}
