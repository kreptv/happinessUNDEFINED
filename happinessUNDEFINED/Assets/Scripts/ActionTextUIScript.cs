using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionTextUIScript : MonoBehaviour
{
    #region Singleton
    public static ActionTextUIScript instance;

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

    private bool broadcastReady;

    void Start()
    {
        broadcastReady = true;
        this.gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }

    public void BroadcastAction(string message, bool errorMessage)
    {
        if (broadcastReady)
        {
            this.gameObject.GetComponent<TextMeshProUGUI>().text = message;

            if (errorMessage)
            {
                this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
            }
            else
            {
                this.gameObject.GetComponent<TextMeshProUGUI>().color = Color.white;
            }

            this.gameObject.GetComponent<Animator>().SetTrigger("NewAction");

            StartCoroutine(WaitAndProceed());
        }
    }


    private IEnumerator WaitAndProceed()
    {
        
        broadcastReady = false;

        // Wait for half a second
        yield return new WaitForSeconds(0.75f);

        broadcastReady = true;
    }



}
