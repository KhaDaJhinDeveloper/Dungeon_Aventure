using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
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
        InventoryDataManager.S_inventoryDataManager.LoadDataItems(this.slotsList);
    }
    public List<InventorySlotItems> SlotsList { get => this.slotsList; set => this.slotsList = value; }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            InventoryDataManager.S_inventoryDataManager.SaveDataItems(this.slotsList);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            InventoryDataManager.S_inventoryDataManager.DeleteAllDataItems();
        }
    }
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
}
