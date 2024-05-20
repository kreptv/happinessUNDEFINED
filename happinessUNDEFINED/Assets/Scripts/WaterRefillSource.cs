using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRefillSource : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collider)
    {

        if (OnHandInventory.lastDraggedItem == null) { return; }

        if (OnHandInventory.lastDraggedItem.gameObject != collider.gameObject) { return; }

        Debug.Log("Dropping in water source...");

        if (OnHandInventory.lastDraggedItem.tag == "waterpot")
        {
            Debug.Log("Refilling water bucket");

            OnHandInventory.lastDraggedItem.gameObject.GetComponent<WaterPot>().FillBucket();
        }
    }



}