using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHeat : MonoBehaviour
{
    public static MapHeat Instance;
    public HeatData heatData;
    
    // Start is called before the first frame update
    void Start()
    {
        if(MapManager.Instance.currentGameData != null) 
        {
            heatData = MapManager.Instance.currentGameData.allNeededData.heatData;
        }
        else
        {
            heatData.globalTemperature = 20.0f;
            heatData.heatDecayRadius = 2.0f;
            heatData.heatScore = 20.0f;
            heatData.tempMin = 10.0d;
            heatData.tempMax = 40.0d;
        }

    }

    public void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateTemperature(Tile[] tiles)
    {
        List<GameObject> onlyBuildings = PrepareList();
        heatData.heatScore = 0.0f;
        int count = 0;
        foreach (var tile in tiles) 
        {
            float addTemperature = 0.0f;
            addTemperature = (float) ComputeTemperature(onlyBuildings, tile.TerrainType, tile.X, tile.Y);
            tile.tileHeat.localTemperature = heatData.globalTemperature + addTemperature;
            tile.tileHeat.localTemperature = ControlTemperature(tile.tileHeat.localTemperature);
            CalculateScore(tile.tileHeat.localTemperature);
            count++;
        }
        Debug.Log(count);
        CalculateScore(count);
        Debug.Log("Counted heat for all tiles, average temperature: " + heatData.heatScore);
        Debug.Log("Actual score:" + ReturnHeatScore());
    }

    private void CalculateScore(double temperature)
    {
        heatData.heatScore += temperature;
    }

    private void CalculateScore(int count)
    {
        heatData.heatScore /= count;
    }

    public int ReturnHeatScore()
    {
        int coolingBonus = 0;
        if (heatData.heatScore < heatData.globalTemperature)
        {
            coolingBonus = (int) ( 100 * (heatData.globalTemperature - heatData.heatScore) );
        }
        return coolingBonus + (int) (20000.0d / (heatData.heatScore < heatData.globalTemperature ? heatData.globalTemperature : heatData.heatScore) );
    }

    // Do not use nor uncomment
    private double ComputeTemperature(List<GameObject> buildings, Tile.TERRAIN_TYPE type, int tileX, int tileY) 
    {
        double addTemperature = 0.0f;
        bool selfCover = false;

        foreach (var building in buildings) {
            double buildingHeat = building.GetComponent<BuildingInfo>().heat;
            int width = building.GetComponent<BuildingPosition>().width, length = building.GetComponent<BuildingPosition>().length;
            float xCenter = building.GetComponent<Transform>().position.x + building.GetComponent<BuildingPosition>().x + width / 2.0f;
            float yCenter = building.GetComponent<Transform>().position.z + building.GetComponent<BuildingPosition>().z + length / 2.0f;
            
            if(!selfCover && tileX > xCenter - (width / 2) && tileX < xCenter + (width / 2) && tileY > yCenter - (width / 2) && tileY < yCenter + (width / 2))
            {
                selfCover = true;
            }

            float X = System.Math.Abs(xCenter - tileX), Y = System.Math.Abs(yCenter - tileY);
            X = (width / 2 > X ? 0 : X - width / 2);
            Y = (length / 2 > Y ? 0 : Y - length / 2);
            double diff = System.Math.Sqrt(X*X+Y*Y), heatModSquared = heatData.heatDecayRadius * heatData.heatDecayRadius;
            double heatBase = buildingHeat / (System.Math.PI * heatModSquared);
            addTemperature += heatBase * System.Math.Exp(-diff / heatModSquared);
        }

        if(!selfCover)
        {
            addTemperature = TerrainHeat(type, addTemperature);
        }

        return addTemperature;
    }

    private double TerrainHeat(Tile.TERRAIN_TYPE type, double temperature)
    {
        switch(type)
        {
            case Tile.TERRAIN_TYPE.WATER: 
            {
                return (temperature > 0 ? temperature * 0.6 : temperature * 0.4); 
                break;
            }
            case Tile.TERRAIN_TYPE.GRASS: 
            {
                return (temperature > 0 ? temperature * 0.7 : temperature * 0.5);
                break;
            }
            case Tile.TERRAIN_TYPE.EARTH: 
            {
                return (temperature > 0 ? temperature * 0.9 : temperature * 0.8);
                break;
            }   
            default:
            {
                return temperature;
                break;
            }         
        }
    }

    private List<GameObject> PrepareList()
    {
        List<GameObject> buildingList = new List<GameObject>();
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("budynek01"))
        {
            buildingList.Add(thing);
        }
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("budynek02"))
        {
            buildingList.Add(thing);
        }
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("budynek03"))
        {
            buildingList.Add(thing);
        }
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("budynek04"))
        {
            buildingList.Add(thing);
        }
        foreach (GameObject thing in GameObject.FindGameObjectsWithTag("budynek05"))
        {
            buildingList.Add(thing);
        }
        return buildingList;


    }

    private float ControlTemperature(float temp)
    {
        if(temp < heatData.tempMin)
        {
            temp = (float) heatData.tempMin;
        }
        else if(temp > heatData.tempMax)
        {
            temp = (float) heatData.tempMax;
        }
        return temp;
    }

}
