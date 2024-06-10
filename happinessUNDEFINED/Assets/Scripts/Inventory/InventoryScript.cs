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
    [SerializeField] public Transform sceneItemsTransform; // A transform where world items are placed
    private GameObject[] inventory = new GameObject[5];
    public int selectedIndex = 0; // -1 means no item is selected
    public GameObject selectedItem;
    public static HashSet<Collider> InRangeOfItem;

    [SerializeField] private Image[] inventorySlots;
    [SerializeField] private Sprite blankSprite;

    private bool onPickupCooldown;

    private void Start()
    {
        SelectItem(0);

        InRangeOfItem = new HashSet<Collider>();

        onPickupCooldown = false;
    }

    void Update()
    {
        // Check for item collection
        if ((InRangeOfItem.Count != 0) && Input.GetKeyDown(KeyCode.E) && !onPickupCooldown)
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
                selectedItem = inventory[selectedIndex];
                item.SetActive(false); // Hide the item in the scene
                item.transform.SetParent(pawTransform[i]); // Set parent to paw
                ActionTextUIScript.instance.BroadcastAction("Collected item: " + item.name + " in slot " + (i + 1), false);
                //Debug.Log("Collected item: " + item.name + " in slot " + (i + 1));

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


            // enable fill in GUI if item has a fill property
            if (item.CompareTag("waterpot"))
            {
                inventorySlots[i].transform.parent.GetChild(3).gameObject.SetActive(true);
                UpdateFill(i, item.GetComponent<WaterPot>());
            }

            SelectItem(i);
                return;
            }
        //Debug.Log("Inventory full! Cannot collect item: " + item.name);
        ActionTextUIScript.instance.BroadcastAction("Inventory full! Cannot collect item: " + item.name, true);
    }

    public void UpdateFill(int i, WaterPot waterpot)
    {
        inventorySlots[i].transform.parent.GetChild(3).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)((float)waterpot.currentFill / (float)waterpot.capacity);
    }

    public void SelectItem(int index)
    {
        if (index >= 0 && index < inventory.Length)
        {

            // change color of old inventory box to white
            inventorySlots[selectedIndex].transform.parent.GetChild(2).GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            selectedIndex = index;
            selectedItem = inventory[selectedIndex];

            // change color of new inventory box to green
            inventorySlots[index].transform.parent.GetChild(2).GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);

            inventorySlots[index].transform.parent.GetComponent<Animator>().SetTrigger("Selected");


            for (int i = 0; i < 5; i++) // set all items inactive
            {
                if (inventory[i] != null)
                {
                //Debug.Log("setting " + pawTransform[i].GetChild(0).gameObject.name + " inactive");
                pawTransform[i].GetChild(0).gameObject.SetActive(false);
                }
            }

            if (inventory[index] != null)
            {
                pawTransform[index].GetChild(0).gameObject.SetActive(true); // show item in paw
            }


            // Check all surrounding objects; if selectedItem == null, hide them
            if (selectedItem == null)
            {
                if (InRangeOfItem != null)
                {
                    //Debug.Log("in range of item test");
                    foreach (Collider i in InRangeOfItem)
                    {
                        PlayerMovementScript.instance.ShowActionPopup(i.gameObject.GetComponent<Item>());
                    }
                }
            }
            else
            {
                if (InRangeOfItem != null)
                {
                    foreach (Collider i in InRangeOfItem)
                    {
                        PlayerMovementScript.instance.HideActionPopup(i.gameObject.GetComponent<Item>());
                    }
                }
            }

            if (selectedItem != null)
            {
                if (selectedItem.tag == "harvestitem" || selectedItem.tag == "weapon")
                {
                    Attack.instance.currentWeaponCollider = selectedItem.transform.GetChild(1).GetComponent<Collider>();
                }
                else
                {
                    Attack.instance.currentWeaponCollider = null;
                }
            }










            //ActionTextUIScript.instance.BroadcastAction("Selected item in slot " + (index + 1), false);
            //Debug.Log("Selected item in slot " + (index + 1));
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
            selectedItem = inventory[selectedIndex];
            inventorySlots[selectedIndex].sprite = blankSprite;
            //Debug.Log("Dropped item: " + item.name);
            ActionTextUIScript.instance.BroadcastAction("Dropped item: " + item.name, false);
            item.GetComponent<SpriteRenderer>().sortingOrder = 0;
            //selectedIndex = 1; // No item selected after dropping
            item.transform.rotation = new Quaternion(0, 0, 0, 0);
            item.GetComponent<SpriteRenderer>().flipX = false;

            inventorySlots[selectedIndex].transform.parent.GetChild(3).gameObject.SetActive(false);


            item.transform.localScale = new Vector3(Mathf.Abs(item.transform.localScale.x), Mathf.Abs(item.transform.localScale.y), Mathf.Abs(item.transform.localScale.z));
            item.GetComponent<Item>().ActionPopup.transform.localScale = new Vector3(1, 1, 1);


            // repoll for items
            SelectItem(selectedIndex);




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
            if (itemCollider.CompareTag("item") || itemCollider.CompareTag("waterpot") || itemCollider.CompareTag("seed") || itemCollider.CompareTag("questitem") || itemCollider.CompareTag("weapon") || itemCollider.CompareTag("potion") || itemCollider.CompareTag("harvestitem"))
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


    public void DestroySelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < inventory.Length && inventory[selectedIndex] != null)
        {
            GameObject item = inventory[selectedIndex];
            Debug.Log("Deleting item from inventory: " + item.name);
            // Delete him
            Destroy(item);
            inventory[selectedIndex] = null;
            selectedItem = inventory[selectedIndex];
            inventorySlots[selectedIndex].sprite = blankSprite;
            inventorySlots[selectedIndex].transform.parent.GetChild(3).gameObject.SetActive(false);

            StartCoroutine(PickupCooldown());

        }
    }

    // Define the coroutine
    private IEnumerator PickupCooldown()
    {
        SelectItem(selectedIndex);
        onPickupCooldown = true;
        yield return new WaitForSeconds(0.01f);
        onPickupCooldown = false;


    }



}