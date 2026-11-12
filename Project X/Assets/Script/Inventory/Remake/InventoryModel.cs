using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel 
{
    public const int MAX_SLOT = 25;
    public const int MAX_STACK = 5;
    public InventorySlotData[] slots = new InventorySlotData[MAX_SLOT];
    private SortedSet<int> emptySlots = new SortedSet<int>();
    private Dictionary<string, List<int>> itemSlotsLookup = new();
    public void Initialize()
    {
        for(int i = 0; i < MAX_SLOT; i++)
        {
            this.slots[i] = new InventorySlotData();
            this.slots[i].SetIndex(i);
            this.emptySlots.Add(i);
        }
    }
    public bool AddItem(string itemName, ItemType type, KeyPool keyPool, int amount = 1)
    {
        if(this.itemSlotsLookup.TryGetValue(itemName, out List<int> slotIndices))
        {
            foreach(var index in slotIndices)
            {
                int canAdd = MAX_STACK - this.slots[index].quantity;
                if(canAdd > 0)
                {
                    int addAmount = Mathf.Min(canAdd, amount);
                    this.slots[index].quantity += addAmount;
                    amount -= addAmount;
                    EventManager.Instance.TriggerEvent<int, InventorySlotData>(NameEvent.Event_InventoryView_OnSlotChanged, index, this.slots[index]);
                    if(amount <= 0) return true;
                }
            }
        }
        while(amount > 0 && this.emptySlots.Count > 0)
        {
            int firstEmptyIndex = this.emptySlots.Min;
            this.emptySlots.Remove(firstEmptyIndex);
            int addAmount = Mathf.Min(MAX_STACK, amount);
            this.slots[firstEmptyIndex].SetItem(itemName, type, keyPool, addAmount);
            amount -= addAmount;
            if(!this.itemSlotsLookup.ContainsKey(itemName))
                this.itemSlotsLookup[itemName] = new List<int>();
            this.itemSlotsLookup[itemName].Add(firstEmptyIndex);
            EventManager.Instance.TriggerEvent<int, InventorySlotData>(NameEvent.Event_InventoryView_OnSlotChanged, firstEmptyIndex, this.slots[firstEmptyIndex]);
            if (amount <= 0) return true;
        }
        return amount == 0;
    }
    public void UsedItem(int slotIndex)
    {
        var slot = this.slots[slotIndex];
        if(this.slots[slotIndex].type == ItemType.Material)
        {
            EventManager.Instance.TriggerEvent(NameEvent.Event_LoadSlotItemReportText, "This is material, not usable");
            SoundManager.Instance.PlayMusicSFX(SoundID.SFX_Notification_Warning);
            return;
        }
        RemoveItem(slot);
        EventManager.Instance.TriggerEvent(NameEvent.Event_LoadSlotItemReportText, "Used item");
        SoundManager.Instance.PlayMusicSFX(SoundID.SFX_Interface_EquipItem);
    }
    public void DropItem(int inDex, Vector3 pos)
    {
        var slot = this.slots[inDex];
        if (slot.quantity <= 0) return;
        GameObject itemObj = ObjectPooling.Instance.GetPool(slot.keyPool);
        if(itemObj != null)
        {
            ItemDropSpawn.itemDropSpawn_Instance?.AnimateDrop(itemObj, pos);
            RemoveItem(slot);
        }
    }
    public void RemoveItem(InventorySlotData slot, int amount = 1)
    {
        if (slot.quantity <= 0) return;
        slot.quantity -= amount;
        if (slot.quantity <= 0)
        {
            if (this.itemSlotsLookup.TryGetValue(slot.itemName, out List<int> slotIndices))
            {
                if(slotIndices.Contains(slot.index))
                {
                    slot.ClearItem();                 
                    slotIndices.Remove(slot.index);
                    this.emptySlots.Add(slot.index);
                }
            }
        }
        EventManager.Instance.TriggerEvent<int, InventorySlotData>(NameEvent.Event_InventoryView_OnSlotChanged, slot.index, this.slots[slot.index]);
    }
}
