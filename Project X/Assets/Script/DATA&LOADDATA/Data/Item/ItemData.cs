using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData 
{
    public int itemId;
    public Vector3 position;
    public ItemData(int itemId, Vector3 position)
    {
        this.itemId = itemId;
        this.position = position;
    }
}

[System.Serializable]
public class AllItems
{
    public List<ItemData> allItemsData = new List<ItemData>();
}


