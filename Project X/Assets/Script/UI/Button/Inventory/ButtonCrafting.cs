using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCrafting : BaseButton
{
    private CraftingItem craftingItem;
    protected override void Start()
    {
        base.Start();
        this.craftingItem = GameObject.FindWithTag(TagManager.TAG_UI).GetComponentInChildren<CraftingItem>();
    }
    protected override void OnClick()
    {
        this.craftingItem.Crafting();
        EventManager.OP_EventManager.TriggerEvent("LoadCraftingeUI");
    }
}
