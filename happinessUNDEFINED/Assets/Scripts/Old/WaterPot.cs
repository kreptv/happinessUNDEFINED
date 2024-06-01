using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPot : Item
{

    public int capacity = 1;
    public int currentFill = 0;

    public void Start()
    {
        Debug.Log(this.itemName + " instantiated as water buckie");
    }


    public void FillBucket()
    {
        if (currentFill < capacity)
        {
            currentFill++;
            Debug.Log("Filled bucket. Current fill: " + currentFill);
        }
        else
        {
            Debug.Log("Tried to fill bucket, but bucket was already full!");
        }
    }

    public bool WaterPlants()
    {
        if (currentFill > 0)
        {
            currentFill--;
            Debug.Log("Watered plants. Current fill: " + currentFill);
            return true;
        }
        else
        {
            Debug.Log("Tried to water plants, but bucket was empty!");
            return false;
        }
    }





}
