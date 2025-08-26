using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
    }    
    private void ShowShop()
    {
        Debug.Log("ShowShop");
    }   
    private void DropItem()
    {
        Debug.Log("DropItem");
    }    
}
