using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class FlowerPlot : MonoBehaviour
{
    [HideInInspector] public Seed seed = null;

    [HideInInspector] public bool inTriggerRange;
    [SerializeField] private GameObject ActionPopup;
    private string lastSuccessfulTest;


    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            inTriggerRange = true;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            inTriggerRange = false;
            ActionPopup.SetActive(false);
        }
    }


    public bool testForWaterpot()
    {
        // must be a seed planted
        if (seed == null) {
            return false;
        }

        // seed must be off cooldown
        if (seed.onStandby){
            return false;
        }

        // seed must not be fully grown
        if (seed.currentGrowth + 1 == seed.GrowingSprite.Length)
        {
            return false;
        }

        // if holding waterpot
        if (InventoryScript.instance.selectedItem == null)
        {
            return false;
        }

        if (InventoryScript.instance.selectedItem.tag == "waterpot")
        {

            // if waterpot does not have a fill
            if (InventoryScript.instance.selectedItem.GetComponent<WaterPot>().currentFill == 0)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    public bool testForSeed()
    {
        // flower pot must be empty
        if (seed != null) { return false; }

        // if holding seed
        if (InventoryScript.instance.selectedItem == null)
        {
            return false;
        }

        if (InventoryScript.instance.selectedItem.tag == "seed")
        {
            return true;
        }
        return false;
    }

    public bool testForHarvestItem()
    {
        // must be a seed planted
        if (seed == null)
        {
            return false;
        }

        // seed must be fully grown
        if (seed.currentGrowth + 1 != seed.GrowingSprite.Length)
        {
            return false;
        }

        // if not holding anything
        if (InventoryScript.instance.selectedItem == null)
        {
            return false;
        }

        // if holding harvestitem
        if (InventoryScript.instance.selectedItem.tag == "harvestitem")
        {
            return true;
        }
        return false;
    }


    private void Update()
    {
        if (inTriggerRange)
        {
            bool waterTest = testForWaterpot();
            bool seedTest = testForSeed();
            bool harvestTest = testForHarvestItem();

            if (waterTest || seedTest || harvestTest)
            {
                ActionPopup.SetActive(true);

                if (waterTest)
                {
                    if (lastSuccessfulTest != "watertest")
                    {
                        //Debug.Log("water test, popup false");
                        // show action popup
                        ActionPopup.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = "Water";
                        ActionPopup.transform.GetChild(0).GetChild(1).GetComponent<TextMeshPro>().text = "E";
                        ActionPopup.SetActive(true);
                        lastSuccessfulTest = "watertest";
                    }


                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ActionTextUIScript.instance.BroadcastAction("Watering plant", false);

                        if (InventoryScript.instance.selectedItem.GetComponent<WaterPot>().WaterPlants())
                        {
                            seed.Grow();
                        }
                    }
                }
                else if (seedTest)
                {

                    if (lastSuccessfulTest != "seedtest")
                    {
                        //Debug.Log("seed test, popup false");
                        // show action popup
                        ActionPopup.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = "Plant";
                        ActionPopup.transform.GetChild(0).GetChild(1).GetComponent<TextMeshPro>().text = "E";
                        ActionPopup.SetActive(true);
                        lastSuccessfulTest = "seedtest";
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ActionTextUIScript.instance.BroadcastAction("Planted seed", false);
                        seed = CopyItem(InventoryScript.instance.selectedItem.GetComponent<Seed>());

                        // set sprite
                        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = this.gameObject.transform.GetChild(0).GetComponent<Seed>().GrowingSprite[0];
                        this.gameObject.transform.GetChild(0).localScale = this.gameObject.transform.GetChild(0).GetComponent<Seed>().GrowthSizes[this.gameObject.transform.GetChild(0).GetComponent<Seed>().currentGrowth];


                        InventoryScript.instance.DestroySelectedItem();
                    }
                }
                else if (harvestTest)
                {

                    if (lastSuccessfulTest != "harvesttest")
                    {
                        //Debug.Log("harvest test, popup false");
                        // show action popup
                        ActionPopup.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = "Harvest";
                        ActionPopup.transform.GetChild(0).GetChild(1).GetComponent<TextMeshPro>().text = "E";
                        ActionPopup.SetActive(true);
                        lastSuccessfulTest = "harvesttest";
                    }

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ActionTextUIScript.instance.BroadcastAction("Harvested plant", false);

                        Debug.Log(this.gameObject.transform.GetChild(0).GetComponent<Seed>().harvestRewards + " " + this.gameObject.GetComponent<Collider>());

                        InventoryScript.instance.selectedItem.GetComponent<HarvestItem>().Harvest(this.gameObject.transform.GetChild(0).GetComponent<Seed>().harvestRewards, this.gameObject.GetComponent<Collider>());


                        // set sprite
                        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                        seed = null;
                    }
                }
            }
            else
            {
                ActionPopup.SetActive(false);
            }
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
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().GrowthPositions = item.GrowthPositions;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().currentGrowth = item.currentGrowth;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().growingCooldown = item.growingCooldown;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().harvestable = item.harvestable;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().harvestRewardsStrings = item.harvestRewardsStrings;

        this.gameObject.transform.GetChild(0).GetComponent<Seed>().harvestRewards = new GameObject[item.harvestRewardsStrings.Length];

        for (int i = 0; i < item.harvestRewardsStrings.Length; i++)
        {
            this.gameObject.transform.GetChild(0).GetComponent<Seed>().harvestRewards[i] = Resources.Load(item.harvestRewardsStrings[i], typeof(GameObject)) as GameObject;
        }

        this.gameObject.transform.GetChild(0).GetComponent<Seed>().growthMoney = item.growthMoney;
        this.gameObject.transform.GetChild(0).GetComponent<Seed>().dropPositionY = item.dropPositionY;


        return this.gameObject.transform.GetChild(0).GetComponent<Seed>();
    }

    public void RemoveSeed()
    {
        seed = null;
    }





}
