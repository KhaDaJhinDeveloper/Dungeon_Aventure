[System.Serializable]
public class DialogEvent 
{
    public DialogEventType eventType;
    //public string eventName;
    public DialogEvent()
    {
        eventType = DialogEventType.None;
    }
}
