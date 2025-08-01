using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNextDialog : BaseButton
{
    private DialogManager dialogManager;
    protected override void Start()
    {
        this.dialogManager = GameObject.FindFirstObjectByType<DialogManager>();
    }
    protected override void OnClick()
    {
        
    }
}
