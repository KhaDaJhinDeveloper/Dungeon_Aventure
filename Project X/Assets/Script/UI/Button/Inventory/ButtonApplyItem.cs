using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonApplyItem : BaseButton
{
    [SerializeField] private InventorySlotUI inventorySlotUI;
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnClick()
    {
        base.OnClick();
        EventManager.Instance.TriggerEvent(NameEvent.Event_Inventory_UseItem, this.inventorySlotUI.indexSlot);
    }
}
