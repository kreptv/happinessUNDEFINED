using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementScript : MonoBehaviour
{
    #region Singleton
    public static PlayerMovementScript instance;

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

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;

    [HideInInspector] public static Rigidbody rb;
    private Vector3 movement;
    private Region CurrentRegion;
    private Collider currentRegionMovementBoundary;
    //private static bool canMove;
    private bool isGrounded;
    private bool onAttackCooldown;
    public Animator myAnimator;
    public static bool facingRight;
    //public GameObject inventoryContainer;
    private bool onTeleportCooldown;
    public GameObject LoadingScreen;
    private DoorScript DoorObject; // most recent door accessed

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myAnimator = this.transform.GetChild(0).GetComponent<Animator>();
        facingRight = true;
        onAttackCooldown = false;
        LoadingScreen.SetActive(false);
    }

    void Update()
    {
        if (!KodaManager.instance.kodaCanMove){return;}

        // Get input from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // Normalize the movement vector to prevent faster diagonal movement
        movement = movement.normalized;

        // Set animator parameters for walking and running
        bool isWalking = movement.x != 0 || movement.z != 0;

        if (isGrounded)
        {
            myAnimator.SetTrigger("grounded");
        }

        //myAnimator.SetBool("running", isRunning && isWalking);
        myAnimator.SetBool("walking", isWalking);

        // Jumping logic
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            myAnimator.SetTrigger("jumping");
            
        }

        if (!onAttackCooldown)
        {
            // Attacking logic
            if (InventoryScript.instance.selectedItem == null)
            {
                return;
            }
            if (isGrounded && Input.GetKeyDown(KeyCode.F) && (InventoryScript.instance.selectedItem.tag == "harvestitem" || InventoryScript.instance.selectedItem.tag == "weapon"))
            {
                StartCoroutine(AttackCooldown());
                Attack.instance.attackEnemiesInRange();
                myAnimator.SetTrigger("attacking");
            }
        }













    }

    bool IsAnimationPlaying(string animationName)
    {
        // Check the base layer (index 0) of the Animator
        AnimatorStateInfo stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);

        // Check if the specified animation is playing
        return stateInfo.IsName(animationName);
    }


    // Define the coroutine
    private IEnumerator AttackCooldown()
    {
        onAttackCooldown = true;

        // Wait for seconds
        yield return new WaitForSeconds(Attack.instance.currentWeaponCollider.gameObject.GetComponent<Weapon>().attackCooldown);

        onAttackCooldown = false;
    }

    void FixedUpdate()
    {
        if (onTeleportCooldown)
        {
            rb.gameObject.transform.position = DoorObject.spawnLocation;
        }

        if (!KodaManager.instance.kodaCanMove) { return; }

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        Vector3 desiredPosition = rb.position + movement * speed * Time.fixedDeltaTime;

        // Move the player character
        rb.MovePosition(desiredPosition);

        // Check player facing direction and update the scale
        if (movement.x < 0) // facing left
        {

            if (!facingRight || IsAnimationPlaying("attacking"))
            {
                return;
            }
            transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x + 4, transform.GetChild(0).position.y, transform.GetChild(0).position.z);
            transform.GetChild(0).localScale = new Vector3(-50, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
            facingRight = false;

        }
        else if (movement.x > 0) // facing right
        {
            if (facingRight || IsAnimationPlaying("attacking"))
            {
                return;
            }
            transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x - 4, transform.GetChild(0).position.y, transform.GetChild(0).position.z);
            transform.GetChild(0).localScale = new Vector3(50, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
            facingRight = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("region"))
        {
            CurrentRegion = other.gameObject.GetComponent<Region>();
            currentRegionMovementBoundary = other.gameObject.GetComponent<Region>().regionCollider;
            Debug.Log("Player is in Region " + other.gameObject.GetComponent<Region>().regionName);
            CameraScript.UpdateBoundary(other);

            InventoryScript.instance.sceneItemsTransform = CurrentRegion.regionItems.transform;
            
            //Debug.Log("Updating camera size to " + CurrentRegion.size);
        }
        if (other.CompareTag("item") || other.CompareTag("waterpot") || other.CompareTag("seed") || other.CompareTag("weapon") || other.CompareTag("questitem") || other.CompareTag("potion") || other.CompareTag("harvestitem"))
        {

            Item currentItem = other.gameObject.GetComponent<Item>();

            if (currentItem.inInventory == true) { return; }
            if (!currentItem.collectable == true) { return; }

            InventoryScript.InRangeOfItem.Add(other);

            if (InventoryScript.instance.selectedItem != null) {return;}

            //Debug.Log("PlayerMovementScript: In range of item " + currentItem.itemName);

            // show action popup
            ShowActionPopup(currentItem);
        }
    }


    public void ShowActionPopup(Item currentItem)
    {
        if (currentItem.ActionPopup)
        {
            currentItem.ActionPopup.transform.GetChild(0).GetChild(0).GetComponent<TextMeshPro>().text = "Pick Up";
            currentItem.ActionPopup.transform.GetChild(0).GetChild(1).GetComponent<TextMeshPro>().text = "E";
            currentItem.ActionPopup.SetActive(true);
        }

    }

    public void HideActionPopup(Item currentItem)
    {
        if (currentItem.ActionPopup)
        {
            currentItem.ActionPopup.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("item") || other.CompareTag("waterpot") || other.CompareTag("seed") || other.CompareTag("weapon") || other.CompareTag("questitem") || other.CompareTag("potion") || other.CompareTag("harvestitem"))
        {
            Item currentItem = other.gameObject.GetComponent<Item>();

            // hide action popup
            HideActionPopup(currentItem);

            //Debug.Log("PlayerMovementScript: No longer in range of item " + currentItem.itemName);

            InventoryScript.InRangeOfItem.Remove(other);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
            Debug.Log("Grounded");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
            Debug.Log("Not Grounded");
        }
    }

    public void TeleportToDoor(string door, GameObject thisdoor)
    {
        if (onTeleportCooldown){ return;}
        StartCoroutine(TeleportCooldown());
        DoorObject = System.Array.Find(RegionScript.instance.doorList, item => item.doorName == door);

        DoorObject.gameObject.transform.parent.parent.gameObject.SetActive(true); // set new region active
        thisdoor.transform.parent.parent.gameObject.SetActive(false); // set old region inactive

        // this.gameObject.GetComponent<Collider>().enabled = false;

        

        rb.gameObject.transform.position = DoorObject.spawnLocation;

        // this.gameObject.GetComponent<Collider>().enabled = true;

        Debug.Log("Teleported to " + DoorObject.spawnLocation);
    }


    // Define the coroutine
    private IEnumerator TeleportCooldown()
    {
        KodaManager.instance.worldCanMove = false;
        KodaManager.instance.kodaCanMove = false;
        onTeleportCooldown = true;
        LoadingScreen.SetActive(true);


        // Wait for seconds
        yield return new WaitForSeconds(0.5f);

        onTeleportCooldown = false;
        KodaManager.instance.worldCanMove = true;
        KodaManager.instance.kodaCanMove = true;
        LoadingScreen.SetActive(false);
    }
}