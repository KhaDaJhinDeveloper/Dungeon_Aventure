using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIntro : BaseInteraction
{
    [SerializeField] private DialogData dialogData;
    private DialogManager dialogManager;
    protected override void LoadComponent()
    {
        this.dialogManager = GameObject.FindFirstObjectByType<DialogManager>();
    }
    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) StartIntro();
    }
    public void StartIntro()
    {
        if(!DialogStateManager.dialogState_Instance.IsDialogCompleted(this.dialogData.dialogID))
        {
            this.dialogManager.StartDialogBox(this.dialogData);
        }
    }
}
