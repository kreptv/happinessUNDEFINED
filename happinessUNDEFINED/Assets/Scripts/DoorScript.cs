using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoorScript : MonoBehaviour
{
        public string doorName;
        //[SerializeField] Region doorRegion;
        public string toDoorName;
        [HideInInspector]public PolygonCollider2D thisDoorCollider;

        private void Awake()
        {
            thisDoorCollider = this.gameObject.transform.GetComponent<PolygonCollider2D>();
        }

        public void EnableDoor()
        {
            thisDoorCollider.gameObject.SetActive(true);
        } // Enable Door

        public void DisableDoor()
        {
            thisDoorCollider.gameObject.SetActive(false);
        } // Disable Door

        public void Teleport()
        {
            PlayerMovementScript.TeleportToDoor(toDoorName);
        }

}
