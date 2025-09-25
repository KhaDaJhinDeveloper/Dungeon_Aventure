using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBack : BaseButton
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
}
