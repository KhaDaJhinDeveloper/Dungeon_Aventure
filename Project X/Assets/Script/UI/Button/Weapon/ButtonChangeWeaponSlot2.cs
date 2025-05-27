using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeWeaponSlot2 : BaseButton
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void AddOnClickEvent()
    {
        base.AddOnClickEvent();
    }
    protected override void OnClick()
    {
        EventManager.OP_EventManager.TriggerEvent("ChangeWeaponSlot2");
    }
}
