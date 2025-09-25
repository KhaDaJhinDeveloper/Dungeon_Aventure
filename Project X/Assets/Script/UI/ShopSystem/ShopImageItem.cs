using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopImageItem : MonoBehaviour
{
    private Image imageItem;
    private ShopSlot shopSlot;
    void Start()
    {
        this.imageItem = GetComponent<Image>();
        this.shopSlot = transform.parent.parent.GetComponent<ShopSlot>();
    }
    void Update()
    {
        this.imageItem.sprite = this.shopSlot.imageItem;
    }
}
