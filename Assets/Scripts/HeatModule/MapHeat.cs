using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHeat
{
    public HeatData heatData;
    
    // Start is called before the first frame update
    void Start()
    {
        heatData.globalTemperature = 20.0f;
        heatData.heatDecayRadius = 1.0f;
        heatData.heatScore = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateTemperature(Tile[] tiles, List<GameObject> allObjects)
    {
        List<GameObject> onlyBuildings = PrepareList(allObjects);
        heatData.heatScore = 0.0f;
        int count = 0;
        Debug.Log("Preparation for couting heat!");
        foreach (var tile in tiles) 
        {
            float addTemperature = 0.0f;
            addTemperature = (float) ComputeTemperature(onlyBuildings, tile.TerrainType, tile.X, tile.Y);
            tile.tileHeat.localTemperature = heatData.globalTemperature + addTemperature;
            CalculateScore(tile.tileHeat.localTemperature);
            Debug.Log("For tile, X: " + tile.X + ", Y: " + tile.Y + ", tileTemperature: " + tile.tileHeat.localTemperature);
            count++;
        }
        Debug.Log("Counted all heat, average temperature: " + heatData.heatScore);
        CalculateScore(count);
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
        while(heatData.heatScore < 20.d)
        {
            coolingBonus = 100 * (20.d - heatData.heatScore);
        }
        return coolingBonus + (int) (20000.0d / (heatData.heatScore < 20.d ? 20.d : heatData.heatScore) );
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
                return temperature * 0.6; 
                break;
            }
            case Tile.TERRAIN_TYPE.GRASS: 
            {
                return temperature * 0.7;
                break;
            }
            case Tile.TERRAIN_TYPE.EARTH: 
            {
                return temperature * 0.95;
                break;
            }   
            default:
            {
                return temperature;
                break;
            }         
        }
    }

    private List<GameObject> PrepareList(List<GameObject> prelist)
    {
        List<GameObject> buildingList = new List<GameObject>();
        foreach (GameObject thing in prelist)
        {
            if(thing.layer == 9) 
            {
                buildingList.Add(thing);
            }
        }
        return buildingList;
    }

}
