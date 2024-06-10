using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class QuestObjective
{
    public string description;
    public bool isCompleted;
}

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string description;
    public string assigner;
    public Sprite assignerIcon;
    public bool mainQuest;
    public QuestObjective[] objectives;
    public bool isCompleted;
}
