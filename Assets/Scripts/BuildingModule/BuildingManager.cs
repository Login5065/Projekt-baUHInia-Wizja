﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//!!!!!!!!!!!OBECNIE NIEUZYWANY SKRYPT!!!!!!!!!!!!!

public class BuildingManager : MonoBehaviour
{
    public GameObject[] buildings;

    public BuildingPlacement buildingPlacement;
    // Start is called before the first frame update
    void Start()
    {
        buildingPlacement = GetComponent<BuildingPlacement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            if (GUI.Button(new Rect(Screen.width / 20, Screen.height / 15 + Screen.height / 12 * i, 100, 30), buildings[i].name))
            {
                buildingPlacement.SetItem(buildings[i]);
                
            }
        }

    }








}
