using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelScript : MonoBehaviour
{
    public GameManager gm;
    public PlayerScript ps;
    public GameObject player;
      //public StatsCanvasScript statsCanvasScript;

    public void Travel(int c, int d, Vector3 spawnAt){
      gm.locations[d].SetActive(true);
      gm.locations[c].SetActive(false);
      gm.mapBounds = gm.locations[d].GetComponent<BoxCollider2D>();
      player.GetComponent<Transform>().position = spawnAt;
      //statsCanvasScript.UpdateStats();
      gm.gameState = 1;
    }



    // Start is called before the first frame update
    void Start() {
      player = ps.player;

    } // start

    // Update is called once per frame
    void Update() {

    } // update
}
