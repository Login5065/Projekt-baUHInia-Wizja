using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Buildings
{
    public List<BuildingObjectData> BuildingObjectData = new List<BuildingObjectData>();

}
[System.Serializable]
public class BuildingObjectData
{
    public string name;
    public Vector3 scale;
    public Vector3 position;
    public Quaternion rotation;

    public int price;
    public int limits;
    public float heat;

    public int x;
    public int z;
    public int width;
    public int length;
    public string tag;

    public BuildingObjectData(GameObject go)
    {

        this.name = go.name;
        this.scale = go.transform.localScale;
        this.position = go.transform.position;
        this.rotation = go.transform.rotation;

        this.price = go.GetComponent<BuildingInfo>().price;
        this.limits = go.GetComponent<BuildingInfo>().limits;
        this.heat = go.GetComponent<BuildingInfo>().heat;

        this.x = go.GetComponent<BuildingPosition>().x;
        this.z = go.GetComponent<BuildingPosition>().z;
        this.length = go.GetComponent<BuildingPosition>().length;
        this.width = go.GetComponent<BuildingPosition>().width;
        this.tag = go.tag;

    }


}


public class BuildingManagment : MonoBehaviour
{
    Buildings buildings = new Buildings();
    string json = null;

    public void Add(GameObject g)
    {
        buildings.BuildingObjectData.Add(new BuildingObjectData(g));
        Debug.Log("Saving current Status of buildings");
        Save();
    }
    public void Remove(GameObject g)
    {
        buildings.BuildingObjectData.Remove(new BuildingObjectData(g));
        Save();
    }

    public List<BuildingObjectData> Load()
    {
        buildings = JsonUtility.FromJson<Buildings>(json);
        return buildings.BuildingObjectData;
    }
    public void Save()
    {
        json = JsonUtility.ToJson(buildings);
    }
    public void CheckJsonList()
    {
        buildings = JsonUtility.FromJson<Buildings>(json);
        Debug.Log("Amount of buildings inside " + buildings.BuildingObjectData.Count());
    }
    public Buildings getBuildingData() {
        return buildings;
    }



}
