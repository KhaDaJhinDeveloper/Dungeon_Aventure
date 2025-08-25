using UnityEngine;

[System.Serializable]
public class DialogLine 
{
    [TextArea] public string dialogText;

    [Header("Branching")]
    public bool hasChoice = false;
    public DialogChoice[] choices;
    public int nextDialogIndex = -1;

    [Header("Conditions")]
    public string requiredFlag; 
    public string setFlag;

    [Header("Event")]
    public bool hasEvent ;
    public DialogEvent[] dialogEvents;
    public bool executeEventOnStart ;
}
