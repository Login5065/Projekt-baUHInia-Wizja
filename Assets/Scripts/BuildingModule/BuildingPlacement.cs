using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private PlaceableBuildings placeableBuildings;
    private Transform currentBuilding;

    private TileComponent tileHit;
    private BoxCollider BuildingHit;

    private Budget budget;

    private int budAmount = 0;
    private int nisbudAmount = 0;
    private int buffer;

    // Start is called before the first frame update
    void Start()
    {
        budget = FindObjectOfType<Budget>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit rayHit, Mathf.Infinity, LayerMask.GetMask("MapTile")))
        {
            if ((tileHit = rayHit.collider.GetComponent<TileComponent>()) != null)
            {
                int x = tileHit.tile.X;
                int y = tileHit.tile.Y;

                if (currentBuilding != null)
                {
                    currentBuilding.position = new Vector3(x, currentBuilding.position.y, y);
                    //ZEBY NIE BUDOWAC NA WODZIE POTRZEBNA ZMIANA W MAPMODULE
                    //ZEBY BUDOWAC WIEKSZE OBIEKTY POTRZEBNA ZMIANA W MAPMODULE
                    if (currentBuilding.gameObject.tag == "budynek01")
                        buffer = budAmount;
                    if (currentBuilding.gameObject.tag == "budynek02")
                        buffer = nisbudAmount;
                    if (budget.gameObject.GetComponent<Budget>().checkBudget(currentBuilding.gameObject) && budget.gameObject.GetComponent<Budget>().checkLimits(currentBuilding.gameObject, buffer))
                    {
                        currentBuilding.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 1.0f, 0.2f, 0.5f);
                    }
                    else
                    {
                        currentBuilding.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.2f, 0.2f, 0.5f);
                    }

                    //Stawianie budynku
                    if (Input.GetMouseButtonDown(0) && budget.gameObject.GetComponent<Budget>().checkBudget(currentBuilding.gameObject) && budget.gameObject.GetComponent<Budget>().checkLimits(currentBuilding.gameObject, buffer))
                    {
                        currentBuilding.GetComponent<MeshRenderer>().material.color = new Color(0.3f, 0.3f, 0.8f, 1.0f);
                        budget.gameObject.GetComponent<Budget>().build(currentBuilding.gameObject);
                        if (currentBuilding.gameObject.tag == "budynek01")
                        {
                            budAmount++;
                        }
                        if (currentBuilding.gameObject.tag == "budynek02")
                        {
                            nisbudAmount++;
                        }
                        currentBuilding = null;
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
                    {
                        budAmount--;
                    }
                    if (BuildingHit.gameObject.tag == "budynek02")
                    {
                        nisbudAmount--;
                    }
                    Destroy(BuildingHit.gameObject);
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
        currentBuilding = ((GameObject)Instantiate(b)).transform;
        placeableBuildings = currentBuilding.GetComponent<PlaceableBuildings>();
        GameObject.Find("budynki/BuildingJson").GetComponent<BuildingManagment>().Add(b);
        GameObject.Find("budynki/BuildingJson").GetComponent<BuildingManagment>().CheckJsonList();
    }
}
