using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBuyItem : BaseButton
{
    private InventorySlotItems inventorySlotItems;
    protected override void Start()
    {
        base.Start();
        this.inventorySlotItems = GameObject.FindFirstObjectByType<InventorySlotItems>();
    }
    protected override void OnClick()
    {
        this.inventorySlotItems.DropItemSlot();
    }
}
