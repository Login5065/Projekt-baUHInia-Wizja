using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private PlaceableBuildings placeableBuildings;
    private GameObject currentBuilding;
    private GameObject redBuilding;
    private GameObject greenBuilding;
    private GameObject TempBuilding;
    private GameObject obiekt;

    private TileComponent tileHit;
    private BoxCollider BuildingHit;

    private Budget budget;

    private int budAmount = 0;
    private int nisbudAmount = 0;
    private int benchAmount = 0;
    private int treeAmount = 0;
    private int fountainAmount = 0;
    private int buffer;

    // Start is called before the first frame update
    void Start()
    {
        budget = FindObjectOfType<Budget>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBuilding != null)
        {
            redBuilding.SetActive(false);
            greenBuilding.SetActive(false);
            currentBuilding.SetActive(false);
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHit, Mathf.Infinity, LayerMask.GetMask("MapTile")))
            {
                if ((tileHit = rayHit.collider.GetComponent<TileComponent>()) != null)
                {
                    int x = tileHit.tile.X;
                    int y = tileHit.tile.Y;

                    int posX = currentBuilding.GetComponent<BuildingPosition>().x;
                    int posZ = currentBuilding.GetComponent<BuildingPosition>().z;

                    if (currentBuilding != null)
                    {
                        currentBuilding.SetActive(false);
                        int width = currentBuilding.GetComponent<BuildingPosition>().width;
                        int length = currentBuilding.GetComponent<BuildingPosition>().length;

                        if (currentBuilding.gameObject.tag == "budynek01")
                            buffer = budAmount;
                        if (currentBuilding.gameObject.tag == "budynek02")
                            buffer = nisbudAmount;
                        if (currentBuilding.gameObject.tag == "budynek03")
                            buffer = benchAmount;
                        if (currentBuilding.gameObject.tag == "budynek04")
                            buffer = treeAmount;
                        if (currentBuilding.gameObject.tag == "budynek05")
                            buffer = fountainAmount;
                        if (budget.gameObject.GetComponent<Budget>().checkBudget(currentBuilding.gameObject) && 
                            budget.gameObject.GetComponent<Budget>().checkLimits(currentBuilding.gameObject, buffer)
                            && CanBuild(width, length, x, y))
                        {
                            greenBuilding.SetActive(true);
                            redBuilding.SetActive(false);
                            greenBuilding.GetComponent<Transform>().position = new Vector3(x - posX, currentBuilding.GetComponent<Transform>().position.y, y - posZ); 
                        }
                        else
                        {
                            greenBuilding.SetActive(false);
                            redBuilding.SetActive(true);
                            redBuilding.GetComponent<Transform>().position = new Vector3(x - posX, currentBuilding.GetComponent<Transform>().position.y, y - posZ);
                        }

                        //Stawianie budynku
                        if (Input.GetMouseButtonDown(0) && budget.gameObject.GetComponent<Budget>().checkBudget(currentBuilding.gameObject) 
                            && budget.gameObject.GetComponent<Budget>().checkLimits(currentBuilding.gameObject, buffer)
                            && CanBuild(width, length, x, y))
                        {
                            BuildObject(width, length, x, y);
                            currentBuilding.SetActive(true);
                            currentBuilding.GetComponent<Transform>().position = new Vector3(x - posX, currentBuilding.GetComponent<Transform>().position.y, y - posZ);
                            greenBuilding.SetActive(false);
                            budget.gameObject.GetComponent<Budget>().build(currentBuilding.gameObject);
                            if (currentBuilding.gameObject.tag == "budynek01")
                                budAmount++;
                            if (currentBuilding.gameObject.tag == "budynek02")
                                nisbudAmount++;
                            if (currentBuilding.gameObject.tag == "budynek03")
                                benchAmount++;
                            if (currentBuilding.gameObject.tag == "budynek04")
                                treeAmount++;
                            if (currentBuilding.gameObject.tag == "budynek05")
                                fountainAmount++;
                            currentBuilding = null;
                            Destroy(redBuilding);
                            Destroy(greenBuilding);
                            redBuilding = null;
                            greenBuilding = null;
                        }
                    }

                }
            }
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHitBuilding, Mathf.Infinity, LayerMask.GetMask("Buildings")))
        {
            if ((BuildingHit = rayHitBuilding.collider.GetComponent<BoxCollider>()) != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    budget.gameObject.GetComponent<Budget>().destroy(BuildingHit.gameObject);
                    if (BuildingHit.gameObject.tag == "budynek01")
                        budAmount--;
                    if (BuildingHit.gameObject.tag == "budynek02")
                        nisbudAmount--;
                    if (BuildingHit.gameObject.tag == "budynek03")
                        benchAmount--;
                    if (BuildingHit.gameObject.tag == "budynek04")
                        treeAmount--;
                    if (BuildingHit.gameObject.tag == "budynek05")
                        fountainAmount--;
                    //uwaga na zamiane y -> z
                    int demolishX = (int)BuildingHit.GetComponent<Transform>().position.x + BuildingHit.GetComponent<BuildingPosition>().x;
                    int demolishY = (int)BuildingHit.GetComponent<Transform>().position.z + BuildingHit.GetComponent<BuildingPosition>().z;
                    int demolishWidth = BuildingHit.GetComponent<BuildingPosition>().width;
                    int demolishLength = BuildingHit.GetComponent<BuildingPosition>().length;
                    Demolish(demolishWidth, demolishLength, demolishX, demolishY);
                    Destroy(BuildingHit.gameObject);             
                    currentBuilding = null;
                }
            }
        }
    }

    //ZOSATNIE ZASTAPIONA PO ZMIANACH W MODULE MAP
    bool IsLegalPosition()
    {
        if (placeableBuildings.colliders.Count >0)
        {
            return false;
        }

        return true;
    }

    //ABY DODAC OBIEKT NALEZY GO UMIESCIC W BUTTON-ie W GUI
    public void SetItem(GameObject b)
    {
        currentBuilding = Instantiate(b);
        placeableBuildings = currentBuilding.GetComponent<PlaceableBuildings>();
        GameObject.Find("budynki/BuildingJson").GetComponent<BuildingManagment>().Add(b);
        GameObject.Find("budynki/BuildingJson").GetComponent<BuildingManagment>().CheckJsonList();
    }
    public void SetItemGreen(GameObject green)
    {
        greenBuilding = Instantiate(green);
    }
    public void SetItemRed(GameObject red)
    {
        redBuilding = Instantiate(red);
    }


    bool CanBuild(int width, int length, int tileHitWidth, int tileHitLength)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                int tempX = tileHitWidth + i;
                int tempY = tileHitLength + j;
                obiekt = GameObject.Find("TILE: " + tempX + "," + tempY);              
                if(obiekt.GetComponent<TileComponent>().tile.objectPlaced == true)
                {
                    return false;
                }
            }
        }
        return true;
    }
    void BuildObject(int width, int length, int tileHitWidth, int tileHitLength)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                int tempX = tileHitWidth + i;
                int tempY = tileHitLength + j;
                obiekt = GameObject.Find("TILE: " + tempX + "," + tempY);
                obiekt.GetComponent<TileComponent>().tile.objectPlaced = true;
            }
        }
    }
    void Demolish(int width, int length, int demolishX, int demolishY)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                int tempX = demolishX + i;
                int tempY = demolishY + j;
                obiekt = GameObject.Find("TILE: " + tempX + "," + tempY);
                obiekt.GetComponent<TileComponent>().tile.objectPlaced = false;
            }
        }
    }
}
