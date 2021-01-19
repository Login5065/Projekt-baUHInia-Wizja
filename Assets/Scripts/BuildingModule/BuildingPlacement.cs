using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private PlaceableBuildings placeableBuildings;
    public GameObject currentBuilding;
    public GameObject redBuilding;
    public GameObject greenBuilding;
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
    private int krok = 1;

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
            Rotation();
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
                            //MapHeat.CalculateTemperature(GameMap.MapData.tiles, BuildingManagment.buildings);
                            currentBuilding = null;
                            Destroy(redBuilding);
                            Destroy(greenBuilding);
                            redBuilding = null;
                            greenBuilding = null;
                            krok = 1;
                        }
                    }

                }
            }
            if(Input.GetMouseButtonDown(0) && currentBuilding != null)
            {
                Destroy(currentBuilding);
                Destroy(redBuilding);
                Destroy(greenBuilding);
                currentBuilding = null;
                redBuilding = null;
                greenBuilding = null;
                krok = 1;
            }
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHitBuilding, Mathf.Infinity, LayerMask.GetMask("Buildings")))
        {
            if ((BuildingHit = rayHitBuilding.collider.GetComponent<BoxCollider>()) != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    
                    //uwaga na zamiane y -> z
                    int demolishX = (int)BuildingHit.GetComponent<Transform>().position.x + BuildingHit.GetComponent<BuildingPosition>().x;
                    int demolishY = (int)BuildingHit.GetComponent<Transform>().position.z + BuildingHit.GetComponent<BuildingPosition>().z;
                    int demolishWidth = BuildingHit.GetComponent<BuildingPosition>().width;
                    int demolishLength = BuildingHit.GetComponent<BuildingPosition>().length;
                    Demolish(demolishWidth, demolishLength, demolishX, demolishY);
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
                    //MapHeat.CalculateTemperature();
                    FindObjectOfType<BuildingManagment>().Remove(BuildingHit.gameObject);
                    Destroy(BuildingHit.gameObject);                                                
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            if (currentBuilding != null)
            {
                Destroy(currentBuilding);
                Destroy(redBuilding);
                Destroy(greenBuilding);
                currentBuilding = null;
                redBuilding = null;
                greenBuilding = null;
                krok = 1;
            }
        }
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
    void Rotation()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            BuildingRotationStep(krok);
            krok++;
            if (krok == 5) krok = 1;
        }
    }
    void BuildingRotationStep(int i)
    {
        int temp = currentBuilding.GetComponent<BuildingPosition>().width;
        currentBuilding.GetComponent<BuildingPosition>().width = currentBuilding.GetComponent<BuildingPosition>().length;
        currentBuilding.GetComponent<BuildingPosition>().length = temp;
        switch (i)
        {
            case 1:
            {
                currentBuilding.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
                redBuilding.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
                greenBuilding.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
                    if (currentBuilding.gameObject.tag == "budynek01")
                    {
                        currentBuilding.GetComponent<BuildingPosition>().z = -7;
                        currentBuilding.GetComponent<BuildingPosition>().x = 0;                      
                    }
                    if (currentBuilding.gameObject.tag == "budynek02")
                    {
                        currentBuilding.GetComponent<BuildingPosition>().z = -3;
                        currentBuilding.GetComponent<BuildingPosition>().x = -2;
                    }
                    if (currentBuilding.gameObject.tag == "budynek03")
                    {

                    }
                    if (currentBuilding.gameObject.tag == "budynek04")
                    {

                    }
                    if (currentBuilding.gameObject.tag == "budynek05")
                    {

                    }
            }
            break;
        case 2:
            {
                currentBuilding.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
                redBuilding.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
                greenBuilding.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
                    if (currentBuilding.gameObject.tag == "budynek01")
                    {
                        currentBuilding.GetComponent<BuildingPosition>().z = -11;
                        currentBuilding.GetComponent<BuildingPosition>().x = -7;
                    }
                    if (currentBuilding.gameObject.tag == "budynek02")
                    {
                        currentBuilding.GetComponent<BuildingPosition>().z = -4;
                        currentBuilding.GetComponent<BuildingPosition>().x = -3;
                    }
                    if (currentBuilding.gameObject.tag == "budynek03")
                    {

                    }
                    if (currentBuilding.gameObject.tag == "budynek04")
                    {

                    }
                    if (currentBuilding.gameObject.tag == "budynek05")
                    {

                    }
                }
            break;
        case 3:
            {
                currentBuilding.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
                redBuilding.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
                greenBuilding.GetComponent<Transform>().Rotate(new Vector3(0, 90, 0));
                    if (currentBuilding.gameObject.tag == "budynek01")
                    {
                        currentBuilding.GetComponent<BuildingPosition>().z = 1;
                        currentBuilding.GetComponent<BuildingPosition>().x = -11;
                    }
                    if (currentBuilding.gameObject.tag == "budynek02")
                    {
                        currentBuilding.GetComponent<BuildingPosition>().z = 0;
                        currentBuilding.GetComponent<BuildingPosition>().x = -4;
                    }
                    if (currentBuilding.gameObject.tag == "budynek03")
                    {

                    }
                    if (currentBuilding.gameObject.tag == "budynek04")
                    {

                    }
                    if (currentBuilding.gameObject.tag == "budynek05")
                    {

                    }
                }
            break;
        case 4:
            {
                currentBuilding.GetComponent<Transform>().Rotate(new Vector3(0, -270, 0));
                redBuilding.GetComponent<Transform>().Rotate(new Vector3(0, -270, 0));
                greenBuilding.GetComponent<Transform>().Rotate(new Vector3(0, -270, 0));
                    if (currentBuilding.gameObject.tag == "budynek01")
                    {
                        currentBuilding.GetComponent<BuildingPosition>().z = 0;
                        currentBuilding.GetComponent<BuildingPosition>().x = 1;
                    }
                    if (currentBuilding.gameObject.tag == "budynek02")
                    {
                        currentBuilding.GetComponent<BuildingPosition>().z = -2;
                        currentBuilding.GetComponent<BuildingPosition>().x = 0;
                    }
                    if (currentBuilding.gameObject.tag == "budynek03")
                    {

                    }
                    if (currentBuilding.gameObject.tag == "budynek04")
                    {

                    }
                    if (currentBuilding.gameObject.tag == "budynek05")
                    {

                    }
                }
            break;      
        }
    }
}
