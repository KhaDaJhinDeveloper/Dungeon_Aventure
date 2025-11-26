using System.Collections.Generic;

[System.Serializable]
public class InventorySlotSaveData
{
    public string nameItemData;
    public ItemType typeData;
    public InventorySlotSaveData() { }
    public InventorySlotSaveData(string nameItemData, ItemType typeData)
    {
        this.nameItemData = nameItemData;
        this.typeData = typeData;
    }
}



[System.Serializable]
public class InventorySaveData
{
    public List<InventorySlotSaveData> slotsData = new List<InventorySlotSaveData>();
}
