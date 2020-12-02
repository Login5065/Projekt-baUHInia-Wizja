using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceableBuildings : MonoBehaviour
{
    [HideInInspector]
    public List<Collider> colliders = new List<Collider>();

    

    private bool isSelected;
    private GameObject buildingSelected;
    // Start is called before the first frame update
    void OnGUI()
    { 
        
            if (GUI.Button(new Rect(100, 200, 100, 30), name)) {
              
            }           
        
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Building")
        {

            colliders.Add(c);
        }
    }
    void OnTriggerExit(Collider c) {
        if (c.tag == "Building")
        {

            colliders.Remove(c);
        }
    }

    public void SetSelected(bool s)
    {
        isSelected = s;
    }
    public void SetBuilding(GameObject gameObject)
    {
        buildingSelected = gameObject;
    }
}
