using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestItem : Item
{

    public void Start()
    {
        Debug.Log(this.itemName + " instantiated as harvest item");
    }


    public void Harvest(GameObject[] harvestRewards, Collider targetCollider)
    {
        // Ensure prefabs array and targetCollider are assigned
        if (harvestRewards.Length == 0 || targetCollider == null)
        {
            Debug.LogError("Prefabs array or targetCollider is not assigned.");
            return;
        }

        // Calculate the center of the collider
        Vector3 center = targetCollider.bounds.center;

        // Instantiate each prefab in a circle
        for (int i = 0; i < harvestRewards.Length; i++)
        {
            float angle = i * Mathf.PI * 2 / harvestRewards.Length; // Calculate angle for each prefab
            Vector3 position = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 3f + center; // Calculate position

            Instantiate(harvestRewards[i], position, Quaternion.identity); // Instantiate prefab
        }

        ActionTextUIScript.instance.BroadcastAction("Harvested plant", false);
    }

}

