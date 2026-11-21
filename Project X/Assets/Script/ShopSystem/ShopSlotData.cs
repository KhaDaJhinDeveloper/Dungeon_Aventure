using UnityEngine;

public class ShopSlotData 
{
    public string itemName;
    public int price;
    public ItemType type;
    public KeyPool key;
    public string description;
    public int index;
    public void SetIndex(int index) => this.index = index;
    public bool IsEmpty => string.IsNullOrEmpty(this.itemName);
    public void SetData(ItemDetailsSO itemDetails, float scalePrice, float priceResonance = 0)
    {
        this.itemName = itemDetails.nameItem;
        int initialPrice = itemDetails.buyPrice + Mathf.RoundToInt(itemDetails.buyPrice * scalePrice);
        this.price = initialPrice + Mathf.RoundToInt(initialPrice * priceResonance);
        this.type = itemDetails.itemType;
        this.key = itemDetails.keyPool;
        this.description = itemDetails.description;
    }
    public void Clear()
    {
        this.itemName = null;
    }
}
