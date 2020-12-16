using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    public float price;
    public int max;
    public float heat;
    public void changeProperty(float newPrice, float newHeat)
    {
        price = newPrice;
        heat = newHeat;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
