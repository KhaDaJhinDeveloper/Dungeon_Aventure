using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogTrigger : BaseInteraction
{
    [SerializeField] private DialogData[] dialogDataNPC;
    private DialogManager dialogManager;
    private bool canDialog;
    protected override void LoadComponent()
    {
        this.dialogManager = GameObject.FindFirstObjectByType<DialogManager>();
    }
    protected override void Update()
    {
        if (this.canDialog)
        {
            if (Input.GetKeyDown(KeyCode.E))
                TriggerDialog();
        }
        else
            this.dialogManager.CloseDialogBox();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_ShowButtonTrigger);
            this.canDialog = true;
        }    
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_HiddenButtonTrigger);
            this.canDialog = false;
        }
    }
    public void TriggerDialog()
    {
        foreach (var dialog in this.dialogDataNPC)
        {
            bool isDialogComplated = this.dialogDataNPC != null && DialogStateManager.dialogState_Instance.IsDialogCompleted(dialog.dialogID);
            if (!dialog.hasunlockFlag)
            {
                if (isDialogComplated)
                {
                    if (dialog.canRepeat)
                        //int line = Mathf.Min(0, dialog.dialogLines.Length - 1);
                        this.dialogManager.StartDialogBox(dialog);
                }
                else
                {
                    this.dialogManager.StartDialogBox(dialog);
                    break;
                }
            }
            else
            {     
                if (DialogStateManager.dialogState_Instance.HasFlag(dialog.unlockFlag))
                {
                    if (!isDialogComplated)
                        this.dialogManager.StartDialogBox(dialog);
                    else
                    {
                        if(dialog.canRepeat)
                            this.dialogManager.StartDialogBox(dialog);
                    }
                }
            }
        }
    }
}
