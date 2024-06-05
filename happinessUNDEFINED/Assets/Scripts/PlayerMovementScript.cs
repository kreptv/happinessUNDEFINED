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
    public float attackCooldown = 5f;

    [HideInInspector] public static Rigidbody rb;
    private Vector3 movement;
    private Collider isTouchingDoor = null;
    private Region CurrentRegion;
    private Collider currentRegionMovementBoundary;
    private static bool canMove;
    private bool isGrounded;
    private bool onAttackCooldown;
    public Animator myAnimator;
    public static bool facingRight;
    //public GameObject inventoryContainer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
        myAnimator = this.transform.GetChild(0).GetComponent<Animator>();
        facingRight = true;
        onAttackCooldown = false;
    }

    void Update()
    {
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

        // Check if player is on Door tile
        if (isTouchingDoor != null)
        {
            // Play a door animation

            // Check if the Enter key is pressed
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Going through door.");
                isTouchingDoor.gameObject.transform.GetComponent<DoorScript>().Teleport();
            }
        }

        // Jumping logic
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            myAnimator.SetTrigger("jumping");
            
        }

        /*if (Input.GetKeyDown(KeyCode.C))
        {
            if (!myAnimator.GetBool("fallToKnees"))
            {
                myAnimator.SetBool("fallToKnees", true);
            }
            else
            {
                myAnimator.SetBool("fallToKnees", false);
            }
        }*/

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


    // Define the coroutine
    private IEnumerator AttackCooldown()
    {
        onAttackCooldown = true;

        // Wait for seconds
        yield return new WaitForSeconds(attackCooldown);

        onAttackCooldown = false;
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;
        Vector3 desiredPosition = rb.position + movement * speed * Time.fixedDeltaTime;

        // Move the player character
        rb.MovePosition(desiredPosition);

        // Check player facing direction and update the scale
        if (movement.x < 0) // facing left
        {
            if (!facingRight)
            {
                return;
            }
            transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x + 4, transform.GetChild(0).position.y, transform.GetChild(0).position.z);
            transform.GetChild(0).localScale = new Vector3(-50, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
            facingRight = false;

        }
        else if (movement.x > 0) // facing right
        {
            if (facingRight)
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
        if (other.CompareTag("door"))
        {
            isTouchingDoor = other;
            Debug.Log("Player is touching a door.");
        }
        if (other.CompareTag("region"))
        {
            CurrentRegion = other.gameObject.GetComponent<Region>();
            currentRegionMovementBoundary = other.gameObject.GetComponent<Region>().regionCollider;
            //Debug.Log("Player is in Region " + other.gameObject.GetComponent<Region>().regionName);
            CameraScript.UpdateBoundary(other);
            CurrentRegion.UpdateCameraSize();
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
        if (other.CompareTag("door"))
        {
            isTouchingDoor = null;
            Debug.Log("Player is no longer touching the door.");
        }
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

    public static void TeleportToDoor(string door)
    {
        DoorScript DoorObject = System.Array.Find(RegionScript.doorList, item => item.doorName == door);

        rb.gameObject.transform.position = new Vector3(DoorObject.thisDoorCollider.gameObject.transform.position.x, DoorObject.thisDoorCollider.gameObject.transform.position.y, rb.gameObject.transform.position.z);
        Debug.Log("Teleported to " + DoorObject.thisDoorCollider.gameObject.transform.position.x + " " + DoorObject.thisDoorCollider.gameObject.transform.position.y);
    }
}