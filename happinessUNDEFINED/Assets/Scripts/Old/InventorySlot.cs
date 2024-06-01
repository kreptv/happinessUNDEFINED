using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using System.Collections.Generic;


public class InventorySlot : EventTrigger, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Item containsItem = null;
    //public CanvasGroup canvas;

    public override void OnDrop(PointerEventData eventData)
    {
        if (OnHandInventory.draggingItem != null)
        { 

            if (containsItem == null && OnHandInventory.draggingItem.collectable) // add to inv slot
            {

                containsItem = CopyItem(OnHandInventory.draggingItem);

                // set inv sprite
                this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = this.gameObject.transform.GetChild(0).GetComponent<Item>().inventorySprite;


                // destroy gameobject if the item came from world
                if (OnHandInventory.draggingItem.inInventory == false)
                {
                    Destroy(OnHandInventory.draggingItem.gameObject);
                }


            }
        }

        Debug.Log("Dropping an item in inventory...");

    }


    public Item CopyItem(Item item)
    {
        this.gameObject.transform.GetChild(0).GetComponent<Item>().itemName = item.itemName;
        this.gameObject.transform.GetChild(0).GetComponent<Item>().desc = item.desc;
        this.gameObject.transform.GetChild(0).GetComponent<Item>().inventorySprite = item.inventorySprite;
       // this.gameObject.transform.GetChild(0).GetComponent<Item>().worldPrefab = Resources.Load(item.itemName, typeof(GameObject)) as GameObject;
        this.gameObject.transform.GetChild(0).GetComponent<Item>().collectable = item.collectable;
        this.gameObject.transform.GetChild(0).GetComponent<Item>().inInventory = true;
        this.gameObject.transform.GetChild(0).GetComponent<Item>().dropPositionY = item.dropPositionY;

        return this.gameObject.transform.GetChild(0).GetComponent<Item>();
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        //canvas.blocksRaycasts = false;
        if (containsItem != null)
        {
            Debug.Log("begin drag");
            OnHandInventory.draggingItem = containsItem;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {

        if (containsItem != null)
        {
            //Debug.Log("dragging");
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        //canvas.blocksRaycasts = true;

        if (containsItem != null)
        {
            Debug.Log("end drag");
            Vector3 dropPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            // if we drop it in a new inventory slot
            if (IsPointerOverUIElement())
            {
                Debug.Log("Moving to new Inventory slot");
            }
            else // if we drop it in world space
            {
                Debug.Log("Dropping in world space");
                dropPosition = PlayerMovementScript.rb.gameObject.transform.position;
                dropPosition.z -= 1;
                dropPosition.y = containsItem.dropPositionY; // Ensure the item is dropped on the ground level
                //GameObject newInstance = Instantiate(containsItem.worldPrefab, dropPosition, Quaternion.identity);
               // newInstance.GetComponent<Item>().inInventory = false;
                //OnHandInventory.lastDraggedItem = newInstance.GetComponent<Item>();
            }

            this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = null; // Remove item from UI
            containsItem = null;
            OnHandInventory.draggingItem = null;
        }

    }

    private bool IsPointerOverUIElement()
    {
        // Get the current mouse position
        Vector2 mousePosition = Input.mousePosition;

        List<GameObject> uiElementsToCheck = new List<GameObject>();
        // List off all of the Inventory slots
        uiElementsToCheck.Add(this.gameObject.transform.parent.GetChild(0).gameObject);
        uiElementsToCheck.Add(this.gameObject.transform.parent.GetChild(1).gameObject);
        uiElementsToCheck.Add(this.gameObject.transform.parent.GetChild(2).gameObject);
        uiElementsToCheck.Add(this.gameObject.transform.parent.GetChild(3).gameObject);
        uiElementsToCheck.Add(this.gameObject.transform.parent.GetChild(4).gameObject);

        // Create a PointerEventData object for the raycast
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = mousePosition
        };

        // Raycast to all UI elements
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        Canvas canvas = this.gameObject.transform.parent.parent.GetComponent<Canvas>();
        GraphicRaycaster graphicRaycaster = canvas.GetComponent<GraphicRaycaster>();
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        // Check if the raycast hit any of the specified UI elements
        foreach (RaycastResult result in raycastResults)
        {
            if (uiElementsToCheck.Contains(result.gameObject))
            {
                return true;
            }
        }

        return false;
    }


}
