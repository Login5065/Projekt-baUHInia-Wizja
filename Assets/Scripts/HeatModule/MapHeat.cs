using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHeat
{
    public static float globalTemperature { get; set; }
    private float heatDecayRadius { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        globalTemperature = 20.0f;
        heatDecayRadius = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float CalculateTemperature(Tile[] tiles, Buildings buildings)
    {
        foreach (var tile in tiles) {
            float addTemperature = 0.0f;
            //addTemperature = ComputeTemperature(buildings.BuildingList, tile.X, tile.Y);
            //addTemperature = TerrainHeat(tile.TerrainType, temperature);
            tile.tileHeat.localTemperature = globalTemperature + addTemperature;
        }
        return 0.0f;
    }

    public float ReturnHeatScore(Tile[] tiles)
    {
        float score = 0;
        int index = 0;
        foreach (var tile in tiles) {
            score += tile.tileHeat.localTemperature;
            index++;
        }
        score /= (float) (index > 0 ? index : 1);
        return score;
    }

    // Do not use nor uncomment
    /*
    private float ComputeTemperature(List<GameObject> buildings, int X, int Y) 
    {
        foreach (var building in buildings) {
            float buildingHeat = building.BuildingInfo.heat;
            int width = building.BuildingPosition.Width, int height = building.BuildingPosition.Height;
            int xCenter = X + width / 2;
            int yCenter = Y + width / 2;
            int X = abs(xCenter - X);
            int Y = abs(yCenter - Y);
            X = (width > X ? 0 : x - width);
            Y = (height > Y ? 0 : y - width);
            float diff = sqrt(X*X+Y*Y), heatModSquared = heatDecayRadius * heatDecayRadius;
            float heatBase = buildingHeat / (Math.PI * heatModSquared);
            temperature += heatBase * pow(exp, (-diff / heatModSquared));
        }
        return temperature;
    }
    */

    /*
    private float TerrainHeat(TERRAIN_TYPE type, float temperature)
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
    */

}
