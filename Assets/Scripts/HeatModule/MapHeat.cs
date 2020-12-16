using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHeat : MonoBehaviour
{
    private float globalTemperature { get; set; }
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
            //tile.tileHeat.localTemperature = ComputeTemperature();
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

}
