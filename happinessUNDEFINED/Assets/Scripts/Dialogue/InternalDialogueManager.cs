using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InternalDialogueManager : MonoBehaviour
{

    #region Singleton
    public static InternalDialogueManager instance;

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

    public GameObject InternalDialoguePopup;

    public TMP_Text dialogueText;

    public GameObject OnScreenUICanvas;

    private Queue<InternalDialogueLine> lines;
    private InternalDialogueLine currentLine;

    [HideInInspector] public bool inDialogue;

    void Start()
    {
        inDialogue = false;
        lines = new Queue<InternalDialogueLine>();
        InternalDialoguePopup.SetActive(false);
    }

    void Update()
    {
        if (!inDialogue) { return; }

        else if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.Return)))
        {
            DisplayNextLine();
        }
    }


    public void StartDialogue(InternalDialogue dialogue)
    {
        inDialogue = true;
        DialogueManager.instance.SetActiveOnScreenUI(false);
        DialogueManager.instance.GameWorldCanMove(false);

        InternalDialoguePopup.SetActive(true);
        lines.Clear();

        foreach (InternalDialogueLine line in dialogue.lines)
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
        dialogueText.text = currentLine.text;
    }

    void EndDialogue()
    {
        StartCoroutine(DialogueCooldown());
        DialogueManager.instance.SetActiveOnScreenUI(true);
        DialogueManager.instance.GameWorldCanMove(true);
        InternalDialoguePopup.SetActive(false);
    }

    private IEnumerator DialogueCooldown()
    {
        inDialogue = true;
        yield return new WaitForSeconds(0.5f);
        inDialogue = false;


    }



}
