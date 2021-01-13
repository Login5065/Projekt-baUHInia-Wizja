using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHeat : MonoBehaviour
{
    public static float globalTemperature { get; set; }
    private float heatModifier { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        globalTemperature = 20.0f;
        heatModifier = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float CalculateTemperature(Tile[] tiles)
    {
        //List<Building> allBuildings;
        foreach (var tile in tiles) {
            //tile.tileHeat.localTemperature = ComputeTemperature(allBuildings, tile.X, tile.Y);
        }
        return 0.0f;
    }

    public float ReturnHeatScore(Tile[] tiles)
    {
        float score = 0;
        int index = 1;
        foreach (var tile in tiles) {
            //score += tile.tileHeat.localTemperature;
            index++;
        }
        score /= (float) index;
        return score;
    }

    // Do not use nor uncomment
    /*
    private float ComputeTemperature(List<Building> buildings, int X, int Y) 
    {
        float temperature = globalTemperature;
        foreach (var building in buildings) {
            float buildingHeat = building.heat;
            int X = abs(building.tile.X - X);
            int Y = abs(building.tile.Y - Y);
            float diff = sqrt(X*X+Y*Y);
            temperature += heatModifier * pow(exp, -diff);
        }
        return temperature;
    }
    */

}
