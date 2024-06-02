using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    #region Singleton
    public static InventoryScript instance;

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

    [SerializeField] private Transform[] pawTransform; // A transform where collected items will be placed
    [SerializeField] private Transform sceneItemsTransform; // A transform where world items are placed
    private GameObject[] inventory = new GameObject[5];
    private int selectedIndex = 0; // -1 means no item is selected
    public static HashSet<Collider> InRangeOfItem;

    [SerializeField] private Image[] inventorySlots;
    [SerializeField] private Sprite blankSprite;

    private void Start()
    {
        SelectItem(0);

        InRangeOfItem = new HashSet<Collider>();
        
    }

    void Update()
    {
        // Check for item collection
        if ((InRangeOfItem.Count != 0) && Input.GetKeyDown(KeyCode.E))
        {
            CollectItem(TryCollectClosestItem());
        }

        // Check for item selection
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectItem(4);

        // Check for item drop
        if (Input.GetKeyDown(KeyCode.Q)) DropSelectedItem();
    }

    private void CollectItem(GameObject item)
    {
            if (inventory[selectedIndex] == null)
            {
                int i = selectedIndex;
                inventory[i] = item;
                item.SetActive(false); // Hide the item in the scene
                item.transform.SetParent(pawTransform[i]); // Set parent to paw
                Debug.Log("Collected item: " + item.name + " in slot " + (i + 1));

                item.transform.position = new Vector3(PlayerMovementScript.rb.gameObject.transform.position.x, PlayerMovementScript.rb.gameObject.transform.position.y, PlayerMovementScript.rb.gameObject.transform.position.z);
                item.transform.localPosition = new Vector3(item.GetComponent<Item>().pawposition.x, item.GetComponent<Item>().pawposition.y, item.transform.localPosition.z);

            // if player grabs object while facing left, flip object
                if (!PlayerMovementScript.facingRight){
                    item.GetComponent<SpriteRenderer>().flipX = true;
                    //item.transform.localScale = new Vector2(-item.transform.localScale.x, item.transform.localScale.y);
                }

            item.GetComponent<Item>().inInventory = true;

                // disable item's Action Popup
                item.GetComponent<Item>().ActionPopup.SetActive(false);

                // disable collider
                item.GetComponent<Collider>().enabled = false;
            

                
                InRangeOfItem.Remove(item.GetComponent<Collider>());

                item.GetComponent<SpriteRenderer>().sortingOrder = 500;


            // Show item in inventory slot
            inventorySlots[i].sprite = item.GetComponent<Item>().inventorySprite;
                inventorySlots[i].transform.localScale = item.GetComponent<Item>().inventorySpriteSize;

                SelectItem(i);
                return;
            }
        Debug.Log("Inventory full! Cannot collect item: " + item.name);
    }

    private void SelectItem(int index)
    {
        if (index >= 0 && index < inventory.Length)
        {
            // change color of old inventory box to white
            inventorySlots[selectedIndex].transform.parent.GetChild(2).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            selectedIndex = index;

            // change color of new inventory box to green
            inventorySlots[index].transform.parent.GetChild(2).GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);


            for (int i = 0; i < 5; i++) // set all items inactive
            {
                if (inventory[i] != null)
                {
                    Debug.Log("setting " + pawTransform[i].GetChild(0).gameObject.name + " inactive");
                pawTransform[i].GetChild(0).gameObject.SetActive(false);
                }
            }

            if (inventory[index] != null)
            {
                pawTransform[index].GetChild(0).gameObject.SetActive(true); // show item in paw
            }


                Debug.Log("Selected item in slot " + (index + 1));
        }
    }

    private void DropSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < inventory.Length && inventory[selectedIndex] != null)
        {
            GameObject item = inventory[selectedIndex];
            // Drop the item in front of the player
            item.transform.position = new Vector3(PlayerMovementScript.rb.gameObject.transform.position.x, item.GetComponent<Item>().dropPositionY, PlayerMovementScript.rb.gameObject.transform.position.z);
            item.transform.SetParent(sceneItemsTransform); // Set parent to items
            item.SetActive(true); // Show the item in the scene
            item.GetComponent<Item>().inInventory = false;
            item.GetComponent<Collider>().enabled = true; // enable collider
            item.GetComponent<Item>().ActionPopup.SetActive(true);
            inventory[selectedIndex] = null;
            inventorySlots[selectedIndex].sprite = blankSprite;
            Debug.Log("Dropped item: " + item.name);
            item.GetComponent<SpriteRenderer>().sortingOrder = 0;
            //selectedIndex = 1; // No item selected after dropping

            item.GetComponent<SpriteRenderer>().flipX = false;


            item.transform.localScale = new Vector3(Mathf.Abs(item.transform.localScale.x), Mathf.Abs(item.transform.localScale.y), Mathf.Abs(item.transform.localScale.z));
            item.GetComponent<Item>().ActionPopup.transform.localScale = new Vector3(1, 1, 1);





        }
    }

    public GameObject TryCollectClosestItem()
    {
        if (InRangeOfItem.Count == 0)
        {
            Debug.Log("No items in range to collect.");
            return null;
        }

        Collider closestItem = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider itemCollider in InRangeOfItem)
        {
            if (itemCollider.CompareTag("item"))
            {
                float distance = Vector3.Distance(PlayerMovementScript.rb.transform.position, itemCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = itemCollider;
                }
            }
        }

        if (closestItem != null)
        {
            return closestItem.gameObject;
        }
        return null;
    }
}