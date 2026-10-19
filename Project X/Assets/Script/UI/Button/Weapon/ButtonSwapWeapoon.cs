using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonSwapWeapoon : BaseButton
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
        base.OnClick();
        EventManager.Instance?.TriggerEvent(NameEvent.Event_WeaponControll_SwapWeapon);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 10, 40, false, true).SetUpdate(true);
    }
}
