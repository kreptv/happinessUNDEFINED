using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class QuestManager : MonoBehaviour
{

    #region Singleton
    public static QuestManager instance;

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


    public Quest[] quests;

    public GameObject QuestJournal;

    public GameObject NewQuestCanvas;

    private bool QuestJournalOpen;



    public GameObject NewQuestPrefab;
    public Transform ContentTransform;



    private void Start()
    {
        QuestJournalOpen = true;
        OpenQuestJournal();
        NewQuestCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!DialogueManager.instance.inDialogue)
        {
            if (Input.GetKeyDown(KeyCode.J)) OpenQuestJournal(); // Open quest journal with J
        }
    }

    private void OpenQuestJournal()
    {
        if (QuestJournalOpen == false)
        {
            QuestJournal.SetActive(true);
            QuestJournalOpen = true;

            //DialogueManager.instance.SetActiveOnScreenUI(false);
            DialogueManager.instance.GameWorldCanMove(false);

        }
        else
        {
            QuestJournal.SetActive(false);
            QuestJournalOpen = false;

            //DialogueManager.instance.SetActiveOnScreenUI(true);
            DialogueManager.instance.GameWorldCanMove(true);
        }
    }



    public void StartQuest(Quest quest)
    {
        quest.isCompleted = false;
        foreach (QuestObjective objective in quest.objectives)
        {
            objective.isCompleted = false;
        }
        Debug.Log("Quest started: " + quest.questName);

        GameObject instance = Instantiate(NewQuestPrefab, ContentTransform); // instantiate under quest journal
        instance.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.objectives[0].description; // Quest task text
        instance.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = quest.questName; // Quest task text
        instance.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = quest.assigner; // Quest assigner text
        instance.transform.GetChild(0).GetChild(4).GetComponent<Image>().sprite = quest.assignerIcon; // Quest assigner text

        if (quest.mainQuest)
        {
            instance.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
        }
        else
        {
            instance.transform.GetChild(0).GetChild(5).gameObject.SetActive(false);
        }

        NewQuestCanvas.SetActive(true);
        NewQuestCanvas.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = quest.questName; // set new quest canvas
        StartCoroutine(PlayNewQuestCanvasAnimation());
    }

    private IEnumerator PlayNewQuestCanvasAnimation()
    {
        NewQuestCanvas.GetComponent<Animator>().SetTrigger("Idle");

        yield return new WaitForSeconds(2.0f);
        NewQuestCanvas.SetActive(false);
    }




    public void CompleteObjective(Quest quest, int objectiveIndex)
    {
        if (objectiveIndex < quest.objectives.Length)
        {
            quest.objectives[objectiveIndex].isCompleted = true;
            CheckQuestCompletion(quest);
        }
    }

    void CheckQuestCompletion(Quest quest)
    {
        foreach (QuestObjective objective in quest.objectives)
        {
            if (!objective.isCompleted)
                return;
        }

        quest.isCompleted = true;
        Debug.Log("Quest completed: " + quest.questName);
    }
}