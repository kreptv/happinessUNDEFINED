using UnityEngine;

[System.Serializable]
public class InternalDialogueLine
{
    public string text;
}

[CreateAssetMenu(fileName = "NewInternalDialogue", menuName = "InternalDialogue")]
public class InternalDialogue : ScriptableObject
{
    public InternalDialogueLine[] lines;
}
