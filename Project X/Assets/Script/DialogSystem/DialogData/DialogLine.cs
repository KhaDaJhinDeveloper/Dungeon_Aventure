using UnityEngine;

[System.Serializable]
public class DialogLine 
{
    [TextArea] public string dialogText;
    public float displaySpeed = 0.5f;

    [Header("Branching")]
    public bool hasChoice = false;
    public DialogChoice[] choices;
    public int nextDialogIndex = -1;

    [Header("Conditions")]
    public string requiredFlag; 
    public string setFlag; 
}
