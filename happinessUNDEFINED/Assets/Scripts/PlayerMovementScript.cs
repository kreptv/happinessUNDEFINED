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
    private static Rigidbody2D rb;
    private Vector2 movement;
    private Collider2D isTouchingDoor = null;
    private Region CurrentRegion;
    private PolygonCollider2D currentRegionMovementBoundary;
    private static bool canMove;

    private Item InRangeOfItem;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    void Update()
    {
        // Get input from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize the movement vector to prevent faster diagonal movement
        movement = movement.normalized;

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
        } // isTouchingDoor

        /*if ((InRangeOfItem != null) && (InRangeOfItem.collectable)){
            // Play an item highlight animation

            // Check if the Enter key is pressed
            if (Input.GetKeyDown(KeyCode.Return))
            {
                InRangeOfItem.CollectItem();
            }



        }*/
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }
        
        Vector2 desiredPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Move the player character
        rb.MovePosition(desiredPosition);
    }


    void OnTriggerEnter2D(Collider2D other)
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
    } // OnTriggerEnter

    void OnTriggerExit2D(Collider2D other)
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
    } // OnTriggerExit




    public static void TeleportToDoor(string door)
    {
        DoorScript DoorObject = System.Array.Find(RegionScript.doorList, item => item.doorName == door);

        rb.gameObject.transform.position = new Vector3(DoorObject.thisDoorCollider.gameObject.transform.position.x, DoorObject.thisDoorCollider.gameObject.transform.position.y, rb.gameObject.transform.position.z);
        Debug.Log("Teleported to " + DoorObject.thisDoorCollider.gameObject.transform.position.x + " " + DoorObject.thisDoorCollider.gameObject.transform.position.y);
    }


}
