using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextNameItem : BaseText
{
    private ShopSlot shopSlot;
    protected override void Start()
    {
        base.Start();
        this.shopSlot = transform.parent.GetComponent<ShopSlot>();
    }
    protected override void Update()
    {
        base.Update();
        this.m_TextMeshProUGUI.text = this.shopSlot.nameItem;
    }
}
