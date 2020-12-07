using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHeat : MonoBehaviour
{
    private float temperature { get; set; }
    private float heatModifier { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        temperature = 20.0f;
        heatModifier = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float CalculateTemperature(Tile[] tiles)
    {
        //List<Building> allBuildings;
        return 0.0f;
    }

}
