using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameManager gm;
    //public DialogueBoxes db;
    public GameObject player;
    public string playerName;

    public int lv; public int xp; public int neededxp; public int health;
    public int damageScaling;

    // Start is called before the first frame update
    void Start()
    {
      PlayerPrefs.SetInt("health", 3);
      PlayerPrefs.SetInt("lv", 0);
      /*health = PlayerPrefs.GetInt("health");
      lv = PlayerPrefs.GetInt("lv");*/
      playerName = player.name;
      PlayerPrefs.SetString("cn", playerName);
      Levels();

    }


    void Levels(){
      neededxp = (lv + 1) * 4;


    }




    // Update is called once per frame
    void Update()
    {

      if (xp >= neededxp){
        lv ++; xp = neededxp - xp;
        PlayerPrefs.SetInt("lv", lv);
        Levels();
        //gm.ss.UpdateStats();
      }


    }
}
