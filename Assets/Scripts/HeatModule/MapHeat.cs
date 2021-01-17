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

    public float CalculateTemperature(Tile[] tiles, Buildings buildings)
    {
        foreach (var tile in tiles) {
            float addTemperature = 0.0f;
            //addTemperature = ComputeTemperature(allBuildings, tile.X, tile.Y);
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
    private float ComputeTemperature(List<Building> buildings, int X, int Y) 
    {
        foreach (var building in buildings) {
            float buildingHeat = building.BuildingInfo.heat;
            int X = abs(building.tile.X - X);
            int Y = abs(building.tile.Y - Y);
            float diff = sqrt(X*X+Y*Y), heatModSquared = heatDecayRadius * heatDecayRadius
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
