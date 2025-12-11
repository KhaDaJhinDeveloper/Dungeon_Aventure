using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour,IDataManager
{
    [SerializeField] private List<InventorySlotItems> slotsList = new List<InventorySlotItems>();
    [SerializeField] private RecoveryItem[] recoveryItems;
    private PlayerStats playerStats;
    private CountdownTimer countdownTimer;
    //------------------------------------
    private void Start()
    {
        this.playerStats = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponent<PlayerStats>();
        this.countdownTimer = GameObject.FindFirstObjectByType<CountdownTimer>();
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_InventorySaveData, SaveData);
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_InventoryLoadData, LoadData);
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_InventoryDeleteData, DeleteData);
    }
    public List<InventorySlotItems> SlotsList { get => this.slotsList; set => this.slotsList = value; }
    public void AddItem(Sprite imageItem, string nameItem, ItemType type)
    {
        foreach (InventorySlotItems item in slotsList)
        {
            if(!item.IsFull)
            {
                item.AddItem(imageItem, nameItem, type);
                item.IsFull = true;
                break;
            }    
        }           
    }
    public void UseItem(string name)
    {
        foreach (RecoveryItem itemSO in recoveryItems)
        {
            if(itemSO.nameItem == name)
            {
                itemSO.ApplyRecovery(this.playerStats, this.countdownTimer);
                break; 
            }
        }           
    }    
    public bool IsFullSlot()
    {
        foreach (InventorySlotItems item in slotsList)
        {
            if (!item.IsFull)
            {
                return false;
            }
        }
        return true;
    }    
    public void DeselectedAllSlots()
    {
        foreach (InventorySlotItems item in slotsList)
        {
            item.SlotSelected.SetActive(false);
            item.Option.SetActive(false);
            item.IsSelected = false;
        }
    }

    public void SaveData()
    {
        InventoryDataManager.S_inventoryDataManager.SaveDataItems(this.slotsList);
    }

    public void LoadData()
    {
        InventoryDataManager.S_inventoryDataManager.LoadDataItems(this.slotsList);
    }

    public void DeleteData()
    {
        InventoryDataManager.S_inventoryDataManager.DeleteAllDataItems();
    }
    public void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_InventorySaveData, SaveData);
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_InventoryLoadData, LoadData);
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_InventoryDeleteData, DeleteData);
    }
}
