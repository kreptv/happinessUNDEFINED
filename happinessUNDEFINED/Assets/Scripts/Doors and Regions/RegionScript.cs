using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionScript : MonoBehaviour
{
    #region Singleton
    public static RegionScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    #endregion

    public Region[] regionList;
    public DoorScript[] doorList;

    void Start()
    {
        // Find all Doors and add them to the DoorScript array
        /*DoorScript[] dinstances = FindObjectsOfType<DoorScript>();

        Debug.Log("Number of instances found: " + dinstances.Length);

        doorList = new DoorScript[dinstances.Length];

        for (int i = 0; i < dinstances.Length; i++)
        {
            doorList[i] = dinstances[i];
            Debug.Log("Added instance: " + doorList[i].gameObject.name);
        }

        // Find all Regions and add them to the Regions array
        Region[] rinstances = FindObjectsOfType<Region>();

        Debug.Log("Number of instances found: " + rinstances.Length);

        regionList = new Region[rinstances.Length];

        for (int i = 0; i < rinstances.Length; i++)
        {
            regionList[i] = rinstances[i];
            Debug.Log("Added instance: " + regionList[i].gameObject.name);
        }*/

    }

}
