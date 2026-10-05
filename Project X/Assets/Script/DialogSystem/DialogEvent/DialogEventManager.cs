using UnityEngine.SceneManagement;

public class DialogEventManager : Singleton<DialogEventManager>
{
    public static DialogEventManager dialogEvent_Instance;
    protected override void Awake()
    {
        dialogEvent_Instance = this;
    }
    public void CallDialogEvent(DialogEvent[] dialogEvent)
    {
        if(dialogEvent == null || dialogEvent.Length ==0) return;
        foreach(DialogEvent e in dialogEvent)
        {
            CallSingleEvent(e);
        } 
            
    }    
    public void CallSingleEvent(DialogEvent dialogEvent)
    {
        switch (dialogEvent.eventType)
        {
            case DialogEventType.None:
                break;
            case DialogEventType.ShowShop:
                ShowShop();
                break;
            case DialogEventType.DropItem:
                DropItem(); 
                break;
            case DialogEventType.NewGame:
                NewGame();
                break;
        }
    }    
    private void ShowShop()
    {
        DebugLogger.Log("ShowShop");
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_OpenShop);
    }   
    private void DropItem()
    {
        DebugLogger.Log("DropItem");
        //Add event drop item
    }
    public void NewGame()
    {
        GameControl.Instance.NewGame(); 
    }    
}
