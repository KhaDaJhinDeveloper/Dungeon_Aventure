using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDropItem : BaseButton
{
    [SerializeField] private InventorySlotUI inventorySlotUI;
    protected override void OnClick()
    {
        base.OnClick();
        EventManager.Instance.TriggerEvent(NameEvent.Event_Inventory_DropItem, this.inventorySlotUI.indexSlot);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }
}
