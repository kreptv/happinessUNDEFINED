using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
        public string itemName;
        public string desc;
        public Sprite inventorySprite;
        public GameObject worldPrefab;
        public bool collectable;
        [HideInInspector] public bool inInventory = false;
         public float dropPositionY;

    private void Awake()
    {
        dropPositionY = this.gameObject.transform.position.y;
    }

}
