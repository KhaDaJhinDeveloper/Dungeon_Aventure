using System.Collections.Generic;

[System.Serializable]
public class InventorySlotSaveData
{
    public string nameItem;
    public int typeData;
    public int keyPool;
    public int quantity;
    public InventorySlotSaveData() { }
    public InventorySlotSaveData(string nameItemData, ItemType typeData, KeyPool key, int quantity)
    {
        this.nameItem = nameItemData;
        this.typeData = (int)typeData;
        this.keyPool = (int)key;
        this.quantity = quantity;
    }
}
[System.Serializable]
public class InventorySaveData
{
    public List<InventorySlotSaveData> InventoryslotsData = new List<InventorySlotSaveData>();
    public List<InventorySlotSaveData> craftingSlots = new();
    public InventorySlotSaveData resultSlot;
}
