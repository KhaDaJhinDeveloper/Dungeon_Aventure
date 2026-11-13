using System.Collections.Generic;
using UnityEngine;
public class InventoryDataManager : Singleton<InventoryDataManager>,IDataManager
{
    private const string SAVE_KEY_ITEMDATA = "InventoryData";

    public void SaveData()
    {
        InventorySaveData save = new();
        InventoryManager inventoryManager = FindFirstObjectByType<InventoryManager>();
        CraftingSystem craftingSystem = FindFirstObjectByType<CraftingSystem>();
        if (inventoryManager == null || craftingSystem == null) return;
        foreach(var slot in inventoryManager.inventoryModel.slots)
        {
            if(!slot.IsEmpty)
            {
                InventorySlotSaveData slotSaveData = new InventorySlotSaveData(slot.itemName, slot.type, slot.keyPool, slot.quantity);
                save.InventoryslotsData.Add(slotSaveData);
            }
        }
        foreach (var slot in craftingSystem.rawMaterials)
        {
            if(!slot.IsEmpty)
            {
                InventorySlotSaveData cartingSlot = new InventorySlotSaveData(slot.itemName, slot.type, slot.keyPool, slot.quantity);
                save.craftingSlots.Add(cartingSlot);
            }
        }
        if(!craftingSystem.itemCompleteEquipment.IsEmpty) save.resultSlot = new InventorySlotSaveData(craftingSystem.itemCompleteEquipment.itemName,
                                                                                                                    craftingSystem.itemCompleteEquipment.type,
                                                                                                                    craftingSystem.itemCompleteEquipment.keyPool,
                                                                                                                    craftingSystem.itemCompleteEquipment.quantity);
        JsonFileUtility.SaveToJson<InventorySaveData>(save, SAVE_KEY_ITEMDATA);
    }

    public void LoadData()
    {
        InventorySaveData data = JsonFileUtility.LoadFromJson<InventorySaveData>(SAVE_KEY_ITEMDATA);
        if (data == null) return;
        InventoryManager inventoryManager = FindFirstObjectByType<InventoryManager>();
        CraftingSystem craftingSystem = FindFirstObjectByType<CraftingSystem>();
        if (inventoryManager == null || craftingSystem == null) return;
        foreach(var slotdata in data.InventoryslotsData)
        {
            inventoryManager.TryPickUpItem(slotdata.nameItem,(ItemType)slotdata.typeData, (KeyPool)slotdata.keyPool, slotdata.quantity);
        }
        craftingSystem.SetData(data.craftingSlots, data.resultSlot);
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(SAVE_KEY_ITEMDATA);    
    }

    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(SAVE_KEY_ITEMDATA);
    }
}
