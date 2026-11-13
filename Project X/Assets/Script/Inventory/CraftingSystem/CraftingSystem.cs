using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public const int MAX_CRAFT_SLOTS = 5;
    [SerializeField] CraftingRecipe[] recipes;
    private InventoryManager inventoryManager;
    public InventorySlotData[] rawMaterials;
    public InventorySlotData itemCompleteEquipment;
    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {
        this.inventoryManager = FindFirstObjectByType<InventoryManager>();
    }
    public void Initialize()
    {
        this.rawMaterials = new InventorySlotData[MAX_CRAFT_SLOTS];
        for (int i = 0; i < MAX_CRAFT_SLOTS; i++)
        {
            this.rawMaterials[i] = new InventorySlotData();
            this.rawMaterials[i].SetIndex(i);
        } 
        this.itemCompleteEquipment = new InventorySlotData();
        this.itemCompleteEquipment.SetIndex(0);
    }    
    public bool TryAddMaterial(int inDex)
    {
        InventoryModel model = this.inventoryManager.inventoryModel;
        if (model == null || inDex < 0 || inDex >= model.slots.Length) return false;
        var slot = model.slots[inDex];
        if (slot.IsEmpty) return false;
        InventorySlotData targetSlot = GetDataSlot(slot);
        if(targetSlot == null)
        {
            EventManager.Instance.TriggerEvent(NameEvent.Event_LoadCraftingReportText, "The crafting table is full of materials");
            return false;
        }
        if (targetSlot.IsEmpty)
            targetSlot.SetItem(slot.itemName, slot.type, slot.keyPool);
        else targetSlot.quantity += 1;
        EventManager.Instance.TriggerEvent(NameEvent.Event_LoadSlotsMaterialUI, targetSlot.index, targetSlot);
        return true;
    }
    InventorySlotData GetDataSlot(InventorySlotData data)
    {
        InventorySlotData slotvalid = null;
        for (int i = 0; i < this.rawMaterials.Length; i++)
        {
            if (!this.rawMaterials[i].IsEmpty && this.rawMaterials[i].itemName == data.itemName)
            {
                slotvalid = this.rawMaterials[i];
                slotvalid.SetIndex(i);
                break;
            }
        }
        if (slotvalid == null)
            for (int i = 0; i < this.rawMaterials.Length; i++)
            {
                if (this.rawMaterials[i].IsEmpty)
                {
                    slotvalid = this.rawMaterials[i];
                    slotvalid.SetIndex(i);
                    break;
                }
            }
        return slotvalid;
    }
    public void Crafting()
    {
        if (!this.itemCompleteEquipment.IsEmpty)
        {
            EventManager.Instance.TriggerEvent(NameEvent.Event_LoadCraftingReportText, "Take out the crafted item first");
            return;
        }
        CraftingRecipe craftingRecipe = null;
        foreach (var reg in this.recipes)
        {
            if (reg.IsRecipeValid(this.rawMaterials))
            {
                craftingRecipe = reg;
                break;
            }
        }
        if (craftingRecipe == null)
        {
            EventManager.Instance.TriggerEvent(NameEvent.Event_LoadCraftingReportText, "No suitable recipe found!");
            return;
        }
        foreach (var reg in craftingRecipe.craftIngredients)
        {
            int regQuantity = reg.quantity;
            for (int i = 0; i < this.rawMaterials.Length; i++)
            {
                if (!this.rawMaterials[i].IsEmpty && this.rawMaterials[i].itemName == reg.itemDetail.nameItem)
                {
                    int reduct = Mathf.Min(this.rawMaterials[i].quantity, regQuantity);
                    this.rawMaterials[i].quantity -= reduct;
                    regQuantity -= reduct;
                    if (this.rawMaterials[i].quantity <= 0)
                        this.rawMaterials[i].ClearItem();
                    if (regQuantity <= 0) break;
                }
            }
        }
        this.itemCompleteEquipment.SetItem(craftingRecipe.resultItem.nameItem,
                                            craftingRecipe.resultItem.itemType,
                                            craftingRecipe.resultKeyPool,
                                            craftingRecipe.resultQuantity);
        Debug.Log(itemCompleteEquipment.itemName);
        Debug.Log(itemCompleteEquipment.type);
        Debug.Log(itemCompleteEquipment.keyPool);
        Debug.Log(itemCompleteEquipment.quantity);
        for (int i = 0; i < this.rawMaterials.Length; i++)
            EventManager.Instance.TriggerEvent(NameEvent.Event_LoadSlotsMaterialUI, i, this.rawMaterials[i]);
        EventManager.Instance.TriggerEvent(NameEvent.Event_LoadSlotResultlUI,this.itemCompleteEquipment);
        EventManager.Instance.TriggerEvent(NameEvent.Event_LoadCraftingReportText, "Successfully carfting!!");
    }
    public void GetItemResult()
    {
        if (this.itemCompleteEquipment == null || string.IsNullOrEmpty(this.itemCompleteEquipment.itemName))
        {
            EventManager.Instance.TriggerEvent(NameEvent.Event_LoadCraftingReportText, "No items");
            return;
        }
        var item = this.itemCompleteEquipment;
        bool success = this.inventoryManager.TryPickUpItem(item.itemName, item.type, item.keyPool, item.quantity);
        string text = success ? "Added item to inventory" : "Inventory is full";
        if (success)
            this.itemCompleteEquipment.ClearItem();
        EventManager.Instance.TriggerEvent(NameEvent.Event_LoadCraftingReportText, text);
        EventManager.Instance.TriggerEvent(NameEvent.Event_LoadSlotResultlUI, this.itemCompleteEquipment);
    }
    public void ResetAllMaterial()
    {
        for(int i =0; i < this.rawMaterials.Length; i ++)
        {
            if (!this.rawMaterials[i].IsEmpty)
            {
                bool add = this.inventoryManager.TryPickUpItem(this.rawMaterials[i].itemName,
                                                this.rawMaterials[i].type,
                                                this.rawMaterials[i].keyPool,
                                                this.rawMaterials[i].quantity);
                if (add)
                {
                    this.rawMaterials[i].ClearItem();
                    EventManager.Instance.TriggerEvent(NameEvent.Event_LoadSlotsMaterialUI, i, this.rawMaterials[i]);
                }
            }
        }    
    }
    #region Data
    public void SetData(List<InventorySlotSaveData> listData, InventorySlotSaveData result)
    {
        if (listData.Count <= 0) return;
        for(int i =0; i < listData.Count; i++)
        {
            this.rawMaterials[i].SetItem(listData[i].nameItem, (ItemType)listData[i].typeData, (KeyPool)listData[i].keyPool, listData[i].quantity);
            this.rawMaterials[i].SetIndex(i);
            EventManager.Instance.TriggerEvent(NameEvent.Event_LoadSlotsMaterialUI, i, this.rawMaterials[i]);
        }
        if (result != null) this.itemCompleteEquipment.SetItem(result.nameItem, (ItemType)result.typeData, (KeyPool)result.keyPool, result.quantity);
        EventManager.Instance.TriggerEvent(NameEvent.Event_LoadSlotResultlUI, this.itemCompleteEquipment);
    }
    #endregion
}
