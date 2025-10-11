using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBackChangeWeapon : BaseButton, IPointerEnterHandler
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
        EventManager.OP_EventManager.TriggerEvent("Hide");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
