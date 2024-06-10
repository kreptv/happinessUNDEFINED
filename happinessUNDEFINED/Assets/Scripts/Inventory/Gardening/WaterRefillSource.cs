using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRefillSource : MonoBehaviour
{

    [HideInInspector] public bool inTriggerRange;
    [SerializeField] private GameObject ActionPopup;

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            inTriggerRange = true;
        }
    }

    public bool testForWaterpot()
    {
        // if holding waterpot
        if (InventoryScript.instance.selectedItem == null) {
            ActionPopup.SetActive(false);
            return false;
        }

        if (InventoryScript.instance.selectedItem.tag == "waterpot")
        {
            if (ActionPopup.activeSelf == false)
            {
                // show action popup
                ActionPopup.SetActive(true);
            }
            return true;
        }
        ActionPopup.SetActive(false);
        return false;
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            inTriggerRange = false;
            ActionPopup.SetActive(false);
        }

    }

    private void Update()
    {
        if (inTriggerRange)
        {
            if (testForWaterpot())
            {
                //Debug.Log("Waterpot test");
                if (Input.GetKeyDown(KeyCode.E) && KodaManager.instance.kodaCanMove)
                {
                    //Debug.Log("Refilling water bucket");

                    InventoryScript.instance.selectedItem.GetComponent<WaterPot>().FillBucket();
                }
            }



        }


    }



}