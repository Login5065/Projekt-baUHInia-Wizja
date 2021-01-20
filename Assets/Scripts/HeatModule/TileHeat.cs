using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileHeat
{
    public float localTemperature { get; set; }

    public TileHeat()
    {
        localTemperature = MapHeat.Instance.heatData.globalTemperature;
    }
}
