using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogIntro : BaseInteraction
{
    [SerializeField] private DialogData dialogData;
    private DialogManager dialogManager;
    private bool canDialog;
    protected override void LoadComponent()
    {
        this.dialogManager = GameObject.FindFirstObjectByType<DialogManager>();
    }
    protected override void Update()
    {

    }
}
