using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoorScript : MonoBehaviour
{
        public string doorName;
        //[SerializeField] Region doorRegion;
        public string toDoorName;

        public bool doorEnabled = true;
    private bool playerInRange = false;

        public Collider thisDoorCollider;

        public Vector3 spawnLocation;

    private void Awake()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void Start()
        {
            //thisDoorCollider = this.gameObject.transform.GetComponent<Collider>();
        }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = true;
            if (doorEnabled)
            {
                this.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
                // internal dialogue
            }
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (doorEnabled)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Debug.Log("Going through door. Destination : " + toDoorName);
                    Teleport();
                }
            }
            else
            {
                // cant tp
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerInRange = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

    }

    public void EnableDoor()
        {
            doorEnabled = true;
        } // Enable Door

        public void DisableDoor()
        {
            doorEnabled = false;
        } // Disable Door

        public void Teleport()
        {
            PlayerMovementScript.instance.TeleportToDoor(toDoorName, this.gameObject);
        }

}
