using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private PlaceableBuildings placeableBuildings;
    private Transform currentBuilding;

    private TileComponent tileHit;
    private BoxCollider BuildingHit;



    // Start is called before the first frame update
    void Start()
    {

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
                    currentBuilding.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 1.0f, 0.2f, 0.5f);
                    //Stawianie budynku
                    if (Input.GetMouseButtonDown(0))
                    {
                        currentBuilding.GetComponent<MeshRenderer>().material.color = new Color(0.3f, 0.3f, 0.8f, 1.0f);
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
    }
}
