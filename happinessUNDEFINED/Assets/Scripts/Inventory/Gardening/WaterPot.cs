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
            ActionTextUIScript.instance.BroadcastAction("Filled bucket. Current fill: " + currentFill, false);
            //Debug.Log("Filled bucket. Current fill: " + currentFill);

            InventoryScript.instance.UpdateFill(InventoryScript.instance.selectedIndex, this.GetComponent<WaterPot>());
        }
        else
        {
            ActionTextUIScript.instance.BroadcastAction("Bucket is already full!", true);
            //Debug.Log("Tried to fill bucket, but bucket was already full!");
        }
    }

    public bool WaterPlants()
    {
        if (currentFill > 0)
        {
            currentFill--;
            ActionTextUIScript.instance.BroadcastAction("Watered plants. Current fill: " + currentFill, false);
            //Debug.Log("Watered plants. Current fill: " + currentFill);
            InventoryScript.instance.UpdateFill(InventoryScript.instance.selectedIndex, this.GetComponent<WaterPot>());
            return true;
        }
        else
        {
            ActionTextUIScript.instance.BroadcastAction("Water bucket is empty!", true);
            //Debug.Log("Tried to water plants, but bucket was empty!");
            return false;
        }
    }





}
