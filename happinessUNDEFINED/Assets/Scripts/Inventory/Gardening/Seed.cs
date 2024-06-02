using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seed : Item
{

    [HideInInspector] public GameObject worldPrefab;

    public Sprite[] GrowingSprite;
    public Vector2[] GrowthSizes;
    public Vector2[] GrowthPositions;
    public int currentGrowth;
    public float growingCooldown;

    public bool harvestable;
    public string[] harvestRewardsStrings;
    [HideInInspector] public GameObject[] harvestRewards;
    public int growthMoney;

    public bool onStandby;




    public void Start()
    {
       //this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowingSprite[0];
        onStandby = false;
    }



    public void Grow()
    {
        if (onStandby)
        {
            return;
        }
        // grow plant
        currentGrowth++;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = GrowingSprite[currentGrowth];
        this.gameObject.transform.localScale = GrowthSizes[currentGrowth];
        this.gameObject.transform.localPosition = GrowthPositions[currentGrowth];

        if (currentGrowth + 1 == GrowingSprite.Length)
        {
            //Debug.Log("Plant has reached max growth");
            ActionTextUIScript.instance.BroadcastAction("Plant is fully grown, +$" + growthMoney, false);
            Money.AddMoney(growthMoney);
            if (harvestable)
            {

                //Debug.Log("Plant is ready for harvest");
            }
        }
        else
        {
            StartCoroutine(WaitAndProceed());
        }
    }



    // Define the coroutine
    private IEnumerator WaitAndProceed()
    {
        // Print a message before waiting
        Debug.Log("Plant " + this.itemName + " on standby for" + growingCooldown + " seconds...");
        onStandby = true;

        // Wait for 15 seconds
        yield return new WaitForSeconds(growingCooldown);

        // Print a message after waiting
        Debug.Log("Plant " + this.itemName + " off standby.");
        onStandby = false;
    }




}
