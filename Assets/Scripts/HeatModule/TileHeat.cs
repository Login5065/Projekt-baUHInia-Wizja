﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHeat
{
    public float localTemperature { get; set; }

    public TileHeat()
    {
        localTemperature = MapHeat.globalTemperature;
    }

}
