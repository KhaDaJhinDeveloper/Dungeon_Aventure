using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonRefreshShop : BaseButton, IPointerEnterHandler
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnClick()
    {
        base.OnClick();
        EventManager.Instance.TriggerEvent(NameEvent.Event_ShopSystem_RefreshShop);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
