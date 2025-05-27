using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReset : BaseButton
{
    private CraftingItem craftingItem;
    protected override void Start()
    {
        base.Start();
        this.craftingItem = GameObject.FindWithTag(TagManager.TAG_UI).GetComponentInChildren<CraftingItem>();
    }
    protected override void OnClick()
    {
        this.craftingItem.ResetAllMaterial();
        EventManager.OP_EventManager.TriggerEvent("LoadUIDefault");
        EventManager.OP_EventManager.TriggerEvent<string>("LoadCraftingReportText", "Returned raw materials to inventory");
    }
}
