using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonChangeWeaponSlot1 : BaseButton, IPointerEnterHandler
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
        EventManager.Instance?.TriggerEvent<int>(NameEvent.Event_WeaponControll_ChangeSlotWeapon,0);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 10, 40, false, true).SetUpdate(true);
    }
}
