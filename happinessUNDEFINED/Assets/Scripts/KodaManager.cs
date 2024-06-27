using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KodaManager : MonoBehaviour
{

    #region Singleton
    public static KodaManager instance;

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

    // Koda's expression
    public Image KodaImage;

    // Koda's transformation

    // Put Koda on pause
    public bool kodaCanMove = true; // finished?

    // Put everything other than Koda in the game on pause
    public bool worldCanMove = true;

    // Restrict Koda's movement to a specific area

    // Force Koda to have a specific item in his inventory



    // 



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
