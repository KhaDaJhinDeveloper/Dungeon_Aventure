using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotData
{
    public string itemName;
    public int quantity;
    public ItemType type;
    public KeyPool keyPool;
    public int index;
    public bool IsEmpty => string.IsNullOrEmpty(this.itemName) || this.quantity == 0;
    public void SetIndex(int index) => this.index = index;
    public void SetItem(string itemName, ItemType type, KeyPool keyPool, int amount = 1)
    {
        this.itemName = itemName;
        this.quantity = amount;
        this.type = type;
        this.keyPool = keyPool;
    }
    public void ClearItem()
    {
        this.itemName = null;
        this.quantity = 0;
    }
}
