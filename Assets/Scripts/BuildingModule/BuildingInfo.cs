using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    public int price;
    public int limits;
    public float heat;
    public void changeProperty(int newPrice, float newHeat, int newLimits)
    {
        price = newPrice;
        heat = newHeat;
        limits = newLimits;
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