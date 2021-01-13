using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Budget : MonoBehaviour
{

    private int currentAmount;

    void Start()
    {

    }

    public void build(GameObject bud)
    {
        int newBudget = currentAmount - bud.gameObject.GetComponent<BuildingInfo>().price;
        modifyBudget(newBudget);
    }

    public void destroy(GameObject bud)
    {
        int newBudget = currentAmount + bud.gameObject.GetComponent<BuildingInfo>().price;
        modifyBudget(newBudget);
    }

    public bool checkBudget(GameObject bud)
    {
        if (bud.gameObject.GetComponent<BuildingInfo>().price <= currentAmount)
            return true;
        else
            return false;
    }

    public bool checkLimits(GameObject bud, int lim)
    {
        if (bud.gameObject.GetComponent<BuildingInfo>().limits > lim)
            return true;
        else
            return false;
    }

    public void modifyBudget(int newBudget)
    {
        currentAmount = newBudget;
    }

    public int getCurrentAmount()
    {
        return currentAmount;
    }

    public void setCurrentAmount(int newValue)
    {
        currentAmount = newValue;
    }
}
