using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnHandInventory : MonoBehaviour
{

    #region Singleton
    public static OnHandInventory instance;

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

    public static Item draggingItem = null;
    public static Item lastDraggedItem = null;

    public static int money = 0;
    [SerializeField] private static TextMeshProUGUI moneyText;

    private void Start()
    {
        moneyText = this.gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        moneyText.text = "$" + money.ToString();
    }


    public static void AddMoney(int moneyToAdd)
    {
        money += moneyToAdd;
        moneyText.text = "$" + money.ToString();

    }

    public static void SubtractMoney(int moneyToSubtract)
    {
        money -= moneyToSubtract;
        moneyText.text = "$" + money.ToString();

    }


}