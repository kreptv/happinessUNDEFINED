using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Region : MonoBehaviour
{
        public string regionName;
        
        [HideInInspector] public Collider regionCollider;
    // parent of this gameobject is region

    [HideInInspector] public GameObject regionNPCS;
    [HideInInspector] public GameObject regionEnemies;
    [HideInInspector] public GameObject regionItems;
    [HideInInspector] public GameObject regionDoors;

    private void Start()
    {
    }


    private void Awake()
        {
            Debug.Log(regionName + " active!");

        regionNPCS = this.transform.parent.GetChild(2).gameObject;
        regionEnemies = this.transform.parent.GetChild(3).gameObject;
        regionItems = this.transform.parent.GetChild(4).gameObject;
        regionDoors = this.transform.parent.GetChild(5).gameObject;

        Debug.Log(regionItems);

        regionCollider = this.gameObject.transform.GetComponent<Collider>();
            //InventoryScript.instance.sceneItemsTransform = regionItems.transform;
        }

        void ShowRegion()
        {
            regionCollider.gameObject.SetActive(true);
        } // Show Region

        void HideRegion()
        {
            regionCollider.gameObject.SetActive(false);
        } // Hide Region

}
