using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    public static ItemDatabase S_ItemDatabase { get; private set; }
    [SerializeField] private List<ItemDetailsSO> allItems = new List<ItemDetailsSO>();
    public Dictionary<string,ItemDetailsSO> itemDictionary = new Dictionary<string,ItemDetailsSO>();
    protected override void Awake()
    {
        base.Awake();
        S_ItemDatabase = this;
        BuildDictionarry();
    }
    public void BuildDictionarry()
    {
        itemDictionary.Clear();
        foreach(var item in allItems)
        {
            if(item != null && !string.IsNullOrEmpty(item.nameItem))
            {
                if (!this.itemDictionary.ContainsKey(item.nameItem))
                    this.itemDictionary[item.nameItem] = item;
                else DebugLogger.LogWarning($"OverWrite item name found: {item.nameItem}");
            }    
        }
        DebugLogger.Log($"Initialize {itemDictionary.Count} item data");
    }    
    public ItemDetailsSO GetItemByName(string nameItem)
    {
        if (string.IsNullOrEmpty(nameItem))
            return null;
        this.itemDictionary.TryGetValue(nameItem, out ItemDetailsSO item);
        return item;
    }    
    public Sprite GetItemSprite( string nameItem )
    {
        ItemDetailsSO item = GetItemByName(nameItem);
        return item != null ? item.imageItem : null;
    }   
    public ItemType GetItemType(string nameItem)
    {
        ItemDetailsSO item = GetItemByName(nameItem);
        return item != null ? item.typeItem : ItemType.Usable;
    }    
}
