using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public string Kodaexpression;
    public string NPCexpression;
    public string text;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] lines;
}
