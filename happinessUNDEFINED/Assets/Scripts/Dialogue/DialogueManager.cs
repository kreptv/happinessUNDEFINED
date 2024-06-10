using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{

    #region Singleton
    public static DialogueManager instance;

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


    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public Image NPCImage;
    public GameObject dialoguePanel;
    public GameObject dialoguePanelPFP;

    public GameObject OnScreenUICanvas;

    private Queue<DialogueLine> lines;
    private DialogueLine currentLine;

    [HideInInspector] public bool inDialogue;

    void Start()
    {
        inDialogue = false;
        lines = new Queue<DialogueLine>();
        SetActiveOnScreenUI(true);
        GameWorldCanMove(true);
        dialoguePanel.SetActive(false); dialoguePanelPFP.SetActive(false);
    }

    public void SetActiveOnScreenUI(bool truefalse)
    {
        OnScreenUICanvas.transform.GetChild(0).gameObject.SetActive(truefalse); // ItemSlots
        OnScreenUICanvas.transform.GetChild(1).gameObject.SetActive(truefalse); // Health
        OnScreenUICanvas.transform.GetChild(2).gameObject.SetActive(truefalse); // Stamina
        OnScreenUICanvas.transform.GetChild(4).gameObject.SetActive(truefalse); // MoneyText
        OnScreenUICanvas.transform.GetChild(5).gameObject.SetActive(truefalse); // ActionText
    }

    public void GameWorldCanMove(bool truefalse)
    {
        KodaManager.instance.worldCanMove = truefalse;
        KodaManager.instance.kodaCanMove = truefalse;
    }

    void Update()
    {
        if (!inDialogue){return;}

        else if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.Return)))
        {
            DisplayNextLine();
        }
    }


    public void StartDialogue(Dialogue dialogue)
    {
        inDialogue = true;
        SetActiveOnScreenUI(false);
        GameWorldCanMove(false);

        dialoguePanel.SetActive(true); dialoguePanelPFP.SetActive(true);
        lines.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentLine = lines.Dequeue();
        speakerText.text = currentLine.speakerName;
        dialogueText.text = currentLine.text;
    }

    void EndDialogue()
    {
        inDialogue = false;
        SetActiveOnScreenUI(true);
        GameWorldCanMove(true);
        dialoguePanel.SetActive(false); dialoguePanelPFP.SetActive(false);
    }
}