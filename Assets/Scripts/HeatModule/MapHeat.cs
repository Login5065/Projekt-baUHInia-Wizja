﻿using System.Collections;
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

    public float CalculateTemperature(Tile[] tiles, List<GameObject> allObjects)
    {
        List<GameObject> onlyBuildings = prepList(allObjects);
        foreach (var tile in tiles) {
            float addTemperature = 0.0f;
            addTemperature = (float) ComputeTemperature(onlyBuildings, tile.TerrainType, tile.X, tile.Y);
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
    private double ComputeTemperature(List<GameObject> buildings, Tile.TERRAIN_TYPE type, int tileX, int tileY) 
    {
        double addTemperature = 0.0f;

        foreach (var building in buildings) {
            double buildingHeat = building.GetComponent<BuildingInfo>().heat;
            int width = building.GetComponent<BuildingPosition>().width, length = building.GetComponent<BuildingPosition>().length;
            float xCenter = building.GetComponent<Transform>().position.x + building.GetComponent<BuildingPosition>().x + width / 2.0f;
            float yCenter = building.GetComponent<Transform>().position.z + building.GetComponent<BuildingPosition>().z + length / 2.0f;
            float X = System.Math.Abs(xCenter - tileX), Y = System.Math.Abs(yCenter - tileY);
            X = (width / 2 > X ? 0 : X - width / 2);
            Y = (length / 2 > Y ? 0 : Y - length / 2);
            double diff = System.Math.Sqrt(X*X+Y*Y), heatModSquared = heatDecayRadius * heatDecayRadius;
            double heatBase = buildingHeat / (System.Math.PI * heatModSquared);
            addTemperature += heatBase * System.Math.Exp(-diff / heatModSquared);
        }

        if(tileX != 0 || tileY != 0)
        {
            addTemperature = TerrainHeat(type, addTemperature);
        }

        return addTemperature;
    }

    private double TerrainHeat(Tile.TERRAIN_TYPE type, float temperature)
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

    private List<GameObject> prepList(List<GameObject> prelist)
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
