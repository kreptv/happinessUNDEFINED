using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
        public string itemName;
        public string desc;
        public bool collectable;
        public Sprite inventorySprite;
            public Vector2 inventorySpriteSize;

         public Vector2 pawposition;
        //public GameObject worldPrefab;
        [HideInInspector] public bool inInventory = false;
        [HideInInspector] public float dropPositionY;

        public GameObject ActionPopup;

    private void Awake()
    {
        dropPositionY = this.gameObject.transform.position.y;

        ActionPopup.SetActive(false);

    }

}
