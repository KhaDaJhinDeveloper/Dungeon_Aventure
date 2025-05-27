using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDropItem : BaseButton
{
    private InventorySlotItems inventorySlotItems;
    protected override void Start()
    {
        base.Start();
        this.inventorySlotItems = transform.parent.parent.GetComponent<InventorySlotItems>();
    }
    protected override void OnClick()
    {
        this.inventorySlotItems.DropItemSlot();
    }
}
