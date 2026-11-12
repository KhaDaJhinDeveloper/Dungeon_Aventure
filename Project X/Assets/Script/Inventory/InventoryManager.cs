using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;
    [SerializeField] private RecoveryItem[] recoveryItems;
    private Transform playerPos;
    public InventoryModel inventoryModel { get; private set; } = new();
    private CraftingSystem craftingSystem;
    private PlayerStats playerStats;
    private CountdownTimer countdownTimer;
    //------------------------------------
    private void Awake()
    {
        this.playerPos = GameObject.FindWithTag(TagManager.TAG_PLAYER).GetComponent<Transform>();
        this.craftingSystem = FindFirstObjectByType<CraftingSystem>();
        this.playerStats = FindFirstObjectByType<PlayerStats>();
        this.countdownTimer = FindFirstObjectByType<CountdownTimer>();
        this.inventoryModel.Initialize(); //50 Slots
    }
    private void Start()
    {
        EventManager.Instance.Subscribe<int>(NameEvent.Event_Inventory_UseItem, UseItem);
        EventManager.Instance.Subscribe<int>(NameEvent.Event_Inventory_DropItem, DropItem);
        EventManager.Instance.Subscribe<int>(NameEvent.Event_Inventory_SelectCratingItem, AddMaterial);
    }
    //
    public bool TryPickUpItem(string itemName, ItemType type, KeyPool keyPool, int amount = 1)
    {
        bool addSuccess = this.inventoryModel.AddItem(itemName, type, keyPool, amount);
        return addSuccess;
    }
    #region USED ITEM
    private void UseItem(int index)
    {
        if (index > this.inventoryModel.slots.Length) return;
        var slot = this.inventoryModel.slots[index];
        ApplyEffectItem(slot.itemName);
        this.inventoryModel.UsedItem(index);
    }
    public void ApplyEffectItem(string name)
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
    #endregion
    #region DROP ITEM
    public void DropItem(int inDex)
    {
        this.inventoryModel.DropItem(inDex, this.playerPos.position);
    }
    #endregion
    #region CRATING
    void AddMaterial(int inDex)
    {
        var slot = this.inventoryModel.slots[inDex];
        bool addSuccess = this.craftingSystem.TryAddMaterial(inDex);
        if (addSuccess)
            this.inventoryModel.RemoveItem(slot);
    }
    #endregion
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe<int>(NameEvent.Event_Inventory_UseItem, UseItem);
        EventManager.Instance?.Unsubscribe<int>(NameEvent.Event_Inventory_DropItem, DropItem);
        EventManager.Instance?.Unsubscribe<int>(NameEvent.Event_Inventory_SelectCratingItem, AddMaterial);
    }
}
