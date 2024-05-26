using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class FlowerPlot : MonoBehaviour
{
    public Seed seed = null;

    public void OnTriggerEnter(Collider collider)
    {

        Debug.Log("Flower pot activated");

        if (OnHandInventory.lastDraggedItem == null) { return;}

        if (OnHandInventory.lastDraggedItem.gameObject != collider.gameObject) { return ;}

        Debug.Log("Dropping in flower pot...");

        if (OnHandInventory.lastDraggedItem.tag == "seed")
        {
            Debug.Log("Dropping seed in flower pot...");
            if (seed == null)
            {
                Debug.Log("Seed planted");

                seed = CopyItem(OnHandInventory.lastDraggedItem.gameObject.GetComponent<Seed>());

                // set sprite
                this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = this.gameObject.transform.GetChild(0).GetComponent<Seed>().GrowingSprite[0];
                this.gameObject.transform.GetChild(0).localScale = this.gameObject.transform.GetChild(0).GetComponent<Seed>().GrowthSizes[this.gameObject.transform.GetChild(0).GetComponent<Seed>().currentGrowth];


                Destroy(OnHandInventory.lastDraggedItem.gameObject);

            }
            else
            {
                Debug.Log("Flower pot is full");
            }
        }
        else if (OnHandInventory.lastDraggedItem.tag == "waterpot")
        {
            if (seed != null)
            {
                Debug.Log("Watering plants...");
                if (OnHandInventory.lastDraggedItem.gameObject.GetComponent<WaterPot>().WaterPlants())
                {
                    if (seed.currentGrowth+1 != seed.GrowingSprite.Length)
                    {
                        //Debug.Log(seed.currentGrowth + " " + seed.GrowingSprite.Length);
                        seed.Grow();
                        Debug.Log("Seed has grown");
                    }
                }

            }
            else
            {
                Debug.Log("No plants to water");
            }

        }
        else
        {
            Debug.Log("Cannot drop item in flower pot");
        }

    }

    public Seed CopyItem(Seed item)
    {
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().itemName = item.itemName;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().desc = item.desc;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().inventorySprite = item.inventorySprite;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().worldPrefab = Resources.Load(item.itemName, typeof(GameObject)) as GameObject;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().collectable = item.collectable;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().inInventory = false;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().GrowingSprite = item.GrowingSprite;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().GrowthSizes = item.GrowthSizes;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().currentGrowth = item.currentGrowth;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().growingCooldown = item.growingCooldown;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().harvestable = item.harvestable;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().cropName = item.cropName;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().growthWorth = item.growthWorth;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().dropPositionY = item.dropPositionY;


        return this.gameObject.transform.GetChild(0).GetComponent<Seed>();
    }

    public void RemoveSeed()
    {
        seed = null;
    }





}
