using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAddMaterial : BaseButton, IPointerEnterHandler
{
    [SerializeField]private InventorySlotUI slotUI;
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnClick()
    {
        base.OnClick();
        EventManager.Instance.TriggerEvent(NameEvent.Event_Inventory_SelectCratingItem, this.slotUI.indexSlot);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }
}
