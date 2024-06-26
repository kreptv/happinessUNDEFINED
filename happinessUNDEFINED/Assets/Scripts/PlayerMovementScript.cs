using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Collider isTouchingDoor = null;
    private Region CurrentRegion;
    private Collider currentRegionMovementBoundary;
    private static bool canMove;
    private bool isGrounded;
    private Animator myAnimator;
    private bool facingRight;

    private Item InRangeOfItem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        canMove = true;
        myAnimator = this.transform.GetChild(0).GetComponent<Animator>();
        facingRight = true;
    }

    void Update()
    {
        // Get input from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // Normalize the movement vector to prevent faster diagonal movement
        movement = movement.normalized;

        // Set animator parameters for walking and running
        //bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isWalking = movement.x != 0 || movement.z != 0;

        if (isGrounded)
        {
            myAnimator.SetTrigger("grounded");
        }

        //myAnimator.SetBool("running", isRunning && isWalking);
        myAnimator.SetBool("walking", isWalking);
        /*if (isRunning)
        {
            myAnimator.speed = 1f;
        }
        else
        {
            myAnimator.speed = 1.4f;
        }*/

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

        // Check player facing direction and update the scale
        if (movement.x < 0) // facing left
        {
            if (!facingRight)
            {
                return;
            }
            transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x +4, transform.GetChild(0).position.y, transform.GetChild(0).position.z);
            transform.GetChild(0).localScale = new Vector3(-50, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
            facingRight = false;
        }
        else if (movement.x > 0) // facing right
        {
            if (facingRight)
            {
                return;
            }
            transform.GetChild(0).position = new Vector3(transform.GetChild(0).position.x -4, transform.GetChild(0).position.y, transform.GetChild(0).position.z);
            transform.GetChild(0).localScale = new Vector3(50, transform.GetChild(0).localScale.y, transform.GetChild(0).localScale.z);
            facingRight = true;
        }
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
            Debug.Log("Player is in Region " + other.gameObject.GetComponent<Region>().regionName);
            CameraScript.UpdateBoundary(other);
            CurrentRegion.UpdateCameraSize();
            Debug.Log("Updating camera size to " + CurrentRegion.size);
        }
        if (other.CompareTag("item") || other.CompareTag("seed"))
        {
            InRangeOfItem = other.gameObject.GetComponent<Item>();
            Debug.Log("In range of item" + InRangeOfItem.itemName);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("door"))
        {
            isTouchingDoor = null;
            Debug.Log("Player is no longer touching the door.");
        }
        if (other.CompareTag("item"))
        {
            InRangeOfItem = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }

    public static void TeleportToDoor(string door)
    {
        DoorScript DoorObject = System.Array.Find(RegionScript.doorList, item => item.doorName == door);

        rb.gameObject.transform.position = new Vector3(DoorObject.thisDoorCollider.gameObject.transform.position.x, DoorObject.thisDoorCollider.gameObject.transform.position.y, rb.gameObject.transform.position.z);
        Debug.Log("Teleported to " + DoorObject.thisDoorCollider.gameObject.transform.position.x + " " + DoorObject.thisDoorCollider.gameObject.transform.position.y);
    }
}