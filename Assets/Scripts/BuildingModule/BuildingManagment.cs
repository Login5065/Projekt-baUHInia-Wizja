using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Buildings
{
    public List<GameObject> buildingList = new List<GameObject>();
    
}

public class BuildingManagment :MonoBehaviour
{
    Buildings buildings = new Buildings();
    string json = null;
   
    public void Add(GameObject g) {
        buildings.buildingList.Add(g);
        Debug.Log("Saving current Status of buildings");
        Save();
    }
    public void Remove(GameObject g) {
        buildings.buildingList.Remove(g);
        Save();
    }

    public List<GameObject> Load() {
        buildings = JsonUtility.FromJson<Buildings>(json);
        return buildings.buildingList;
    }
    public void Save() {
       json = JsonUtility.ToJson(buildings);
    }
    public void CheckJsonList() {
        buildings = JsonUtility.FromJson<Buildings>(json);
        Debug.Log("Amount of buildings inside " + buildings.buildingList.Count());
    }




}
