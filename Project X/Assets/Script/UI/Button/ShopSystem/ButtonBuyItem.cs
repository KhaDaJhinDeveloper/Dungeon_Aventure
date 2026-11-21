using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBuyItem : BaseButton, IPointerEnterHandler
{
    [SerializeField] private ShopSlotUI shopSlotUI;
    protected override void OnClick()
    {
        base.OnClick();
        BuyItem();
    }
    public void BuyItem()
    {
        EventManager.Instance.TriggerEvent(NameEvent.Event_ShopSystem_BuyItem, this.shopSlotUI.indexSlot);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
