using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryDataManager : Singleton<InventoryDataManager>
{
    public static InventoryDataManager S_inventoryDataManager;
    private InventorySaveData inventorySaveData = new InventorySaveData();
    private const string SAVE_KEY_ITEMDATA = "InventoryData";
    protected override void Awake()
    {
        base.Awake();
        S_inventoryDataManager = this;
    }
    public void SaveDataItems(List<InventorySlotItems> listDataItems)
    {
        this.inventorySaveData.slotsData.Clear();
        foreach (InventorySlotItems item in listDataItems)
        {
            if (item.IsFull && !string.IsNullOrEmpty(KeyClean.CleanKey(item.NameItem)))
                inventorySaveData.slotsData.Add(new InventorySlotSaveData(KeyClean.CleanKey(item.NameItem), item.Type));
        }  
        string json = JsonUtility.ToJson(inventorySaveData);
        PlayerPrefs.SetString(SAVE_KEY_ITEMDATA, json);
        PlayerPrefs.Save();
        DebugLogger.Log($"Inventory saved: {inventorySaveData.slotsData.Count} items");
    }    
    public void LoadDataItems(List<InventorySlotItems> listDataItems)
    {
        if(PlayerPrefs.HasKey(SAVE_KEY_ITEMDATA))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY_ITEMDATA);
            inventorySaveData = JsonUtility.FromJson<InventorySaveData>(json);
            if (this.inventorySaveData.slotsData == null)
            {
                return;
            }
            for (int i = 0; i < inventorySaveData.slotsData.Count; i++)
            {
                InventorySlotSaveData slotdata = inventorySaveData.slotsData[i];
                Sprite sprite = ItemDatabase.S_ItemDatabase.GetItemSprite(slotdata.nameItemData);
                if (sprite != null)
                {
                    listDataItems[i].ImageItem.sprite = sprite;
                    listDataItems[i].NameItem = slotdata.nameItemData;
                    listDataItems[i].Type = slotdata.typeData;
                    listDataItems[i].IsFull = true;
                    DebugLogger.Log($"Loaded item: {slotdata.nameItemData}");
                }
                else
                {
                    DebugLogger.LogWarning($"Could not find sprite for : {slotdata.nameItemData}");
                }
            }    
        }
        else DebugLogger.Log("No inventory data found in PlayerPrefs");
    }    
    public void DeleteAllDataItems()
    {
        this.inventorySaveData.slotsData.Clear();
        PlayerPrefs.DeleteKey(SAVE_KEY_ITEMDATA);
        PlayerPrefs.Save();
        DebugLogger.Log("Inventory data deleted");
    }    
}
