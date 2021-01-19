using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    public int price;
    public int limits;
    public float heat;
    public void changePropertyBudget(int newPrice, int newLimits)
    {
        price = newPrice;
        limits = newLimits;
    }
    public void changePropertyHeat(int newHeat)
    {
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