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
    public int ID;

    public BuildingObjectData(GameObject go)
    {



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

        this.ID = go.GetInstanceID();

    }


}


public class BuildingManagment : MonoBehaviour
{
    Buildings buildings = new Buildings();
    string json = null;

    public void Add(GameObject g)
    {
        buildings.BuildingObjectData.Add(new BuildingObjectData(g));
        Debug.Log("Status of buildings ... nr of buildings " + getCount());
        Save();
    }
    public bool Remove(GameObject g)
    {
        for (int i = 0; i < getCount(); i++)
        {
            if (buildings.BuildingObjectData[i].ID == g.GetInstanceID()) {
                buildings.BuildingObjectData.Remove(buildings.BuildingObjectData[i]);
                Debug.Log("Status of buildings ... nr of buildings " + getCount());
                return true;
            }
        
        }
        return false;
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
    public int getCount() {
        return buildings.BuildingObjectData.Count();
    }



}
