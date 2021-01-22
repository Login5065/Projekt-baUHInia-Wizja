using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class Buildings
{
    public List<BuildingObjectData> BuildingObjectData = new List<BuildingObjectData>();
    public List<BuildingPrefabData> tab = new List<BuildingPrefabData>();
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
        this.position = go.transform.localPosition;
        this.rotation = go.transform.localRotation;

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

[System.Serializable]
public class BuildingPrefabData
{
    public int price = 0;
    public int limits = 0;
    public float heat = 0;

    public BuildingPrefabData(int x, int y, float z)
    {
        this.price = x;
        this.limits = y;
        this.heat = z;
    }
}


    public class BuildingManagment : MonoBehaviour
{
    List<BuildingObjectData> lista;
    List<BuildingPrefabData> Tab;
    Buildings buildings = new Buildings();
    //private GameObject prefab;
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject[] prefabs = new GameObject[5];
    int prefabCount = 0;
    public GameObject FinanceObject;
    private GameObject prefab_1;
    private GameObject prefab_2;
    private GameObject prefabTemp;

    string json = null;


    public void SavePrefab()
    {
        for (int i = 0; i < 5; i++)
        {
            buildings.tab.Add(new BuildingPrefabData(prefabs[i].GetComponent<BuildingInfo>().price, prefabs[i].GetComponent<BuildingInfo>().limits, 
                prefabs[i].GetComponent<BuildingInfo>().heat));
        }
    }

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
        SavePrefab();
        return buildings;
    }
    public int getCount() {
        return buildings.BuildingObjectData.Count();
    }

    public void getLista()
    {
        lista = MapManager.Instance.currentGameData.getBuildingList();
    }
    public void getTab()
    {
        Tab = MapManager.Instance.currentGameData.getFinanceList();
    }

    public void dodajDane(int i, GameObject prefab)
    {
        prefabTemp = prefab;
        prefabTemp.GetComponent<Transform>().localPosition = lista[i].position;
        prefabTemp.GetComponent<Transform>().localRotation = lista[i].rotation;
        prefabTemp.GetComponent<Transform>().localScale = lista[i].scale;
        prefabTemp.GetComponent<BuildingPosition>().x = lista[i].x;
        prefabTemp.GetComponent<BuildingPosition>().z = lista[i].z;
        prefabTemp.GetComponent<BuildingPosition>().width = lista[i].width;
        prefabTemp.GetComponent<BuildingPosition>().length = lista[i].length;
        prefabTemp.GetComponent<BuildingInfo>().price = lista[i].price;
        prefabTemp.GetComponent<BuildingInfo>().limits = lista[i].limits;
        GameObject.Find("budynki/BuildingJson").GetComponent<BuildingManagment>().Add(prefabTemp);
    }
    public void Awake()
    {

        getLista();
        getTab();
        FinanceUpdate();
        BuildingListCount();
        BuildingInstantiate();
        MapHeat.Instance.CalculateTemperature(GameMap.Instance.mapData.tiles);
        FindObjectOfType<BudgetMenager>().setBuildingsPriceLoad();


    }
    private void BuildingListCount()
    {
        foreach(BuildingObjectData list in lista)
        {
            prefabCount++;
        }
    }
    private void BuildingInstantiate()
    {
        for(int i = 0; i<prefabCount; i++)
        {
            if(lista[i].tag == "budynek01")
            {
                dodajDane(i, prefab1);
                Instantiate(prefabTemp);
                FinanceObject.GetComponent<PrefabInfo>().budAmount++;
            }
            if (lista[i].tag == "budynek02")
            {
                dodajDane(i, prefab2);
                Instantiate(prefabTemp);
                FinanceObject.GetComponent<PrefabInfo>().nisbudAmount++;
            }
            if (lista[i].tag == "budynek03")
            {
                dodajDane(i, prefab3);
                Instantiate(prefabTemp);
                FinanceObject.GetComponent<PrefabInfo>().benchAmount++;
            }
            if (lista[i].tag == "budynek04")
            {
                dodajDane(i, prefab4);
                Instantiate(prefabTemp);
                FinanceObject.GetComponent<PrefabInfo>().treeAmount++;
            }
            if (lista[i].tag == "budynek05")
            {
                dodajDane(i, prefab5);
                Instantiate(prefabTemp);
                FinanceObject.GetComponent<PrefabInfo>().fountainAmount++;
            }            
        }
    }

    private void FinanceUpdate()
    {
        for(int i = 0; i<5; i++)
        {
            FinanceObject.GetComponent<PrefabInfo>().price[i] = Tab[i].price;
            FinanceObject.GetComponent<PrefabInfo>().limits[i] = Tab[i].limits;
            FinanceObject.GetComponent<PrefabInfo>().heat[i] = Tab[i].heat;
        }
   }




}
