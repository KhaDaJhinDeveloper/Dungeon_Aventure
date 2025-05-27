using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingItem : MonoBehaviour
{
    [SerializeField] private InventorySlotItems[] rawMaterials;
    [SerializeField] CraftingRecipe[] recipes;
    private InventoryManager inventoryManager;  
    [SerializeField] private InventorySlotItems completeEquipment;
    public InventorySlotItems[] RawMaterials { get => rawMaterials; set => rawMaterials = value; }
    public InventorySlotItems CompleteEquipment { get => completeEquipment; set => completeEquipment = value; }
    private void Start()
    {
        this.inventoryManager = GetComponent<InventoryManager>();
    }
    public void AddMaterial(InventorySlotItems itemMaterial)
    {
        for(int i = 0; i < this.rawMaterials.Length; i++)
        {
            if (!this.rawMaterials[i].IsFull)
            {
                this.rawMaterials[i].ImageItem.sprite = itemMaterial.ImageItem.sprite;
                this.rawMaterials[i].NameItem = itemMaterial.NameItem;
                this.rawMaterials[i].IsFull = true;
                itemMaterial.EmptySlot();
                break;
            }
        }    
    }
    public void ResetAllMaterial()
    {
        for (int i = 0; i < this.rawMaterials.Length; i++)
        {
            if (this.rawMaterials[i].IsFull)
            {
                this.inventoryManager.AddItem(this.rawMaterials[i].ImageItem.sprite, this.rawMaterials[i].NameItem, this.rawMaterials[i].Type);
                this.rawMaterials[i].NameItem = null;
                this.rawMaterials[i].IsFull = false;
            }
        }
    }
    public void GetItemcomplete()
    {
        if (this.completeEquipment.IsFull)
        {
            this.inventoryManager.AddItem(this.completeEquipment.ImageItem.sprite, this.completeEquipment.NameItem, this.completeEquipment.Type);
            this.completeEquipment.EmptySlot();
            EventManager.OP_EventManager.TriggerEvent<string>("LoadCraftingReportText", "Added item to inventory");
        }
        else EventManager.OP_EventManager.TriggerEvent<string>("LoadCraftingReportText", "No items");
    }
    public void Crafting()
    {
        foreach (CraftingRecipe recipe in this.recipes)
        {
            for (int i = 0; i < this.rawMaterials.Length; i++)
            {
                bool match1 = recipe.item1.nameItem == this.rawMaterials[0].NameItem && recipe.item2.nameItem == this.rawMaterials[1].NameItem;
                bool match2 = recipe.item2.nameItem == this.rawMaterials[0].NameItem && recipe.item1.nameItem == this.rawMaterials[1].NameItem;
                if (match1 || match2)
                {
                    this.completeEquipment.NameItem = recipe.result.nameItem;
                    this.completeEquipment.ImageItem.sprite = recipe.result.spriteImage;
                    this.completeEquipment.Type = recipe.result.type;
                    this.completeEquipment.IsFull = true;
                    EventManager.OP_EventManager.TriggerEvent<string>("LoadCraftingReportText", "Finished item crafting");
                    for (i = 0; i < this.rawMaterials.Length; i++)
                    {
                        this.rawMaterials[i].EmptySlot();
                    }
                    break;
                }
                else
                {
                    EventManager.OP_EventManager.TriggerEvent<string>("LoadCraftingReportText", "No matching formula found");
                } 
            }
        }       
    }
}
