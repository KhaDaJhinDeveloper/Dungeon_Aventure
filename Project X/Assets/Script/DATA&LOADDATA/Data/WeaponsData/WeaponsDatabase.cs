using System.Collections.Generic;
using UnityEngine;

public class WeaponsDatabase : Singleton<WeaponsDatabase>
{
    [SerializeField] private List<WeaponData> allItems = new ();
    public Dictionary<string, WeaponData> itemDictionary = new ();
    protected override void Awake()
    {
        base.Awake();
        BuildDictionarry();
    }
    public void BuildDictionarry()
    {
        itemDictionary.Clear();
        foreach (var item in allItems)
        {
            if (item != null && !string.IsNullOrEmpty(item.weaponName))
            {
                if (!this.itemDictionary.ContainsKey(item.weaponName))
                    this.itemDictionary[item.weaponName] = item;
            }
        }
    }
    public WeaponData GetItemByName(string nameItem)
    {
        if (string.IsNullOrEmpty(nameItem))
            return null;
        this.itemDictionary.TryGetValue(nameItem, out WeaponData item);
        return item;
    }
}
