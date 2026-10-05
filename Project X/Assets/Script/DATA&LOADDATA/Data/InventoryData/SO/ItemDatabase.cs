using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    [SerializeField] private List<ItemDetailsSO> allItems = new List<ItemDetailsSO>();
    public Dictionary<string,ItemDetailsSO> itemDictionary = new Dictionary<string,ItemDetailsSO>();
    protected override void Awake()
    {
        base.Awake();
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
            }    
        }
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
