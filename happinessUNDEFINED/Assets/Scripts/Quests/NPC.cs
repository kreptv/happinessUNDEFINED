using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    public Quest quest;

    private DialogueManager dialogueManager;
    private QuestManager questManager;

    public GameObject ActionPopupTransform;

    private bool inRange;

    void Start()
    {
        inRange = false;
        dialogueManager = DialogueManager.instance;
        questManager = QuestManager.instance;
        ActionPopupTransform.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActionPopupTransform.SetActive(true);
            inRange = true;


        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActionPopupTransform.SetActive(false);
            inRange = false;


        }
    }




    private void Update()
    {
        if(!inRange){return;}

        else if(Input.GetKeyDown(KeyCode.E) && !dialogueManager.inDialogue && KodaManager.instance.kodaCanMove) // Player presses E key
        {

            if (InventoryScript.instance.TryCollectClosestItem()) { return; }

            PlayerMovementScript.instance.myAnimator.SetBool("walking", false);

            dialogueManager.StartDialogue(dialogue);

            if (quest != null && !quest.isCompleted)
            {
                questManager.StartQuest(quest);
            }

        }



    }





}