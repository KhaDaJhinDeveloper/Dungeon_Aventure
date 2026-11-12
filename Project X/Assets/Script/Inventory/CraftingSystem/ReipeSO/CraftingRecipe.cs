using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct CraftIngredient
{
    public ItemDetailsSO itemDetail;
    public int quantity;
}
[CreateAssetMenu(menuName = "Crafting/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<CraftIngredient> craftIngredients = new();
    public ItemDetailsSO resultItem;
    public int resultQuantity = 1;
    public KeyPool resultKeyPool;

    public bool IsRecipeValid(InventorySlotData[] inputslots)
    {
        Dictionary<string, int> available = new();
        int activeSlot = 0;
        foreach (InventorySlotData slot in inputslots)
        {
            if(slot!= null &&!slot.IsEmpty)
            {
                activeSlot++;
                if (!available.ContainsKey(slot.itemName))
                    available[slot.itemName] = 0;
                available[slot.itemName] += slot.quantity;
            }    
        }
        foreach(var reg in this.craftIngredients)
        {
            if (reg.itemDetail == null) continue;
            string regName = reg.itemDetail.nameItem;
            if (!available.TryGetValue(regName, out int currentValue) && currentValue < reg.quantity)
                return false;
            available[regName] -= reg.quantity;
        }
        foreach(var kvp in available)
        {
            if (kvp.Value > 0) return false;
        }
        return true;
    }    
}
