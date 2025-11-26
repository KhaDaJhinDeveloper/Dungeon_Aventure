using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDropItem : BaseButton, IPointerEnterHandler
{
    private InventorySlotItems inventorySlotItems;
    protected override void Start()
    {
        base.Start();
        this.inventorySlotItems = transform.parent.parent.GetComponent<InventorySlotItems>();
    }
    protected override void OnClick()
    {
        base.OnClick();
        this.inventorySlotItems.DropItemSlot();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
