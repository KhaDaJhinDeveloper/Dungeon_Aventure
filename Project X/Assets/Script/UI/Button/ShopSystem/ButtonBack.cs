using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonBack : BaseButton, IPointerEnterHandler
{
    private ShopManager shopManager;
    protected override void Start()
    {
        base.Start();
        this.shopManager = GameObject.FindFirstObjectByType<ShopManager>();
    }
    protected override void OnClick()
    {
        base.OnClick();
        this.shopManager.currentSlot = null;
        EventManager.OP_EventManager.TriggerEvent(NameEvent.Event_CloseShop);     
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
