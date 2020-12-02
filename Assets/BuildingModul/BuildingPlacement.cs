using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacement : MonoBehaviour
{
    private PlaceableBuildings placeableBuildings;
    private Transform currentBuilding;
    private bool hasPlaced =false;
    private PlaceableBuildings placeableBuildingsOld;
    public LayerMask BuildingsMask;




    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 m = Input.mousePosition;
        m = new Vector3(m.x,m.y,transform.position.y);
        Vector3 p = GetComponent<Camera>().ScreenToWorldPoint(m);
        
        if (currentBuilding != null && !hasPlaced)
        {
     

          
            currentBuilding.position = new Vector3(p.x,0,p.z);

            if (Input.GetMouseButtonDown(0))
            {
                if (isLegalPosition())
                {
                    hasPlaced = true;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = new RaycastHit();
                Ray ray = new Ray(new Vector3(p.x, 8, p.z), Vector3.down);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildingsMask))
                {
                    if (placeableBuildingsOld != null)
                    {
                        placeableBuildingsOld.SetSelected(false);
                    }

                    hit.collider.gameObject.GetComponent<PlaceableBuildings>().SetSelected(true);
          
                    placeableBuildingsOld = hit.collider.gameObject.GetComponent<PlaceableBuildings>();
                }
                else
                {
                    if (placeableBuildingsOld != null)
                    {
                        placeableBuildingsOld.SetSelected(false);
                    }

                }

            }
            else if (Input.GetMouseButtonDown(1)) {


                Debug.Log("hello");
                RaycastHit hit = new RaycastHit();
                Ray ray = new Ray(new Vector3(p.x, 8, p.z), Vector3.down);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildingsMask))
                {
                    Destroy(hit.collider.gameObject);
                }
            }


        }
    }


    bool isLegalPosition()
    {
        if (placeableBuildings.colliders.Count >0)
        {
            return false;
        }

        return true;
    }


    public void SetItem(GameObject b)
    {
        hasPlaced = false;
        currentBuilding = ((GameObject)Instantiate(b)).transform;
        placeableBuildings = currentBuilding.GetComponent<PlaceableBuildings>();
    }
}
