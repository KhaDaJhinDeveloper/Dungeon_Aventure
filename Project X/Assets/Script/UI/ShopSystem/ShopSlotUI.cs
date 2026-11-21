using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image imageItem;
    [SerializeField] private TextMeshProUGUI nameItem;
    [SerializeField] private TextMeshProUGUI priceItem;
    [SerializeField] private Sprite defaultSprite;
    public int indexSlot {  get; private set; }
    public void SetIndex(int index) => this.indexSlot = index;
    public void UpdateUI(ShopSlotData data)
    {
        if(data.IsEmpty)
        {
            this.imageItem.sprite = this.defaultSprite;
            this.nameItem.text ="??????";
            this.priceItem.text = "??????";
            return;
        }
        Sprite sprite = ItemDatabase.Instance.GetItemSprite(data.itemName);
        this.imageItem.sprite = sprite;
        this.nameItem.text = data.itemName;
        this.priceItem.text = data.price.ToString();
    }
}
