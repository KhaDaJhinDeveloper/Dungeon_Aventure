using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData 
{
    public string nameItem;
    public Vector3 position;
    public ItemData(string nameItem, Vector3 position)
    {
        this.nameItem = nameItem;
        this.position = position;
    }
}

[System.Serializable]
public class AllItems
{
    public List<ItemData> allItemsData = new List<ItemData>();
}


