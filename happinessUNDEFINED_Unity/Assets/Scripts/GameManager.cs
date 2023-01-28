using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


  private static GameManager _instance;

  public static GameManager Instance{
    get{
      if (_instance is null){
        Debug.LogError("GameManager is NULL");
      }
      return _instance;
    }
  }

  private void Awake(){
    _instance = this;
    if (_instance != null && _instance != this){
      Destroy(this);
    }
    else{
      _instance = this;
    }
  }

  /*public PlayerScript ps;
  public TravelScript ts;
  public DialogueScript dias;
  public StatsCanvasScript ss;
  public CombatScript coms;
  public DialogueBoxScript diaboxs;*/

  public BoxCollider2D mapBounds;
  public GameObject[] locations;
  public string[] locationNames;
  public float xMin, xMax, yMin, yMax;

  public bool bindMovement;

  public int timeOfDay;
  // 1 == day
  // 2 == night
  // 3 == recollection
  // 4 == psychedelic

  public int currRoom;

  public int sceneprogress = 0;

  private Scene currentScene;
  private string sceneName;

  public int gameState;
      // 0 == menu
      // 1 == playing; movement enabled, fighting disabled
      // 2 == playing; movement enabled, fighting enabled
      // 3 == playing; movement disabled, dialogue scene
      // 4 == cutscene
      // 5 == travelling

    // Start is called before the first frame update
    void Start()
    {
      DontDestroyOnLoad(this.gameObject);

      PlayerPrefs.GetInt("chapter");
      PlayerPrefs.GetInt("location");
      PlayerPrefs.GetString("locationName");
      PlayerPrefs.GetString("quest");
      PlayerPrefs.GetInt("progress");
      PlayerPrefs.GetString("cn");
      PlayerPrefs.GetInt("lv");
      PlayerPrefs.GetInt("xp");

      //ss.UpdateStats();


      // Determine first scene
      currentScene = SceneManager.GetActiveScene();
      sceneName = currentScene.name;
      if (sceneName == "MenuScene"){
        gameState = 0;
      }
      else{
        gameState = 1;
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;

      }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
