using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
  public GameManager gm;
  public TravelScript ts;
  public bool doorAvailable;
  private bool canTravel;

  public int currRoom;
  public int destination;
  public Vector3 spawnAt;
  [HideInInspector]
  public string currRoomName;
  [HideInInspector]
  public string destinationName;



    // Start is called before the first frame update
    void Start() {
      this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
      canTravel = false;
      gm.mapBounds = gm.locations[currRoom].GetComponent<BoxCollider2D>();
      currRoomName = gm.locationNames[currRoom];
      destinationName = gm.locationNames[destination];
      PlayerPrefs.SetInt("location", currRoom);
      PlayerPrefs.SetString("locationName", currRoomName);
    }

    void Update() {
      if (canTravel == true){
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
              gm.gameState = 5;
        }
      }

      if (gm.gameState == 5){
        PlayerPrefs.SetInt("location", destination);
        PlayerPrefs.SetString("locationName", destinationName);
        ts.Travel(currRoom, destination, spawnAt);
      }

    } // update

    void OnTriggerEnter2D(Collider2D collision){
    if (collision.gameObject.tag == "Player" && doorAvailable == true) {
      this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
      canTravel = true;
      }
    } // on collision

    void OnTriggerExit2D(Collider2D collision){
    if (collision.gameObject.tag == "Player") {
      this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
      canTravel = false;
      }
    } // on collision
}
