using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBuyItem : BaseButton, IPointerEnterHandler
{
    private InventoryManager inventoryManager;
    private ShopSlot shopSlot;
    private CoinManager coinManager;
    [SerializeField] private GameObject slotObject;
    protected override void Start()
    {
        base.Start();
        this.inventoryManager = GameObject.FindFirstObjectByType<InventoryManager>();
        this.coinManager = GameObject.FindFirstObjectByType<CoinManager>();
        this.shopSlot = transform.parent.GetComponent<ShopSlot>();
    }
    protected override void OnClick()
    {
        base.OnClick();
        BuyItem();
    }
    public void BuyItem()
    {
        if(this.coinManager.CoinAmount > this.shopSlot.priceItem)
        {
            if(!this.inventoryManager.IsFullSlot())
            {
                this.inventoryManager.AddItem(this.shopSlot.imageItem,
                                              this.shopSlot.nameItem,
                                              this.shopSlot.typeItem);
                this.coinManager.SpendCoin(this.shopSlot.priceItem);
                this.slotObject.SetActive(false);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
