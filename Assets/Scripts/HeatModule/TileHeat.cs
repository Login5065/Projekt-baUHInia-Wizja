using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHeat : MonoBehaviour
{
    public float localTemperature { get; set; }

    public TileHeat()
    {
        localTemperature = MapHeat.globalTemperature;
    }
}
