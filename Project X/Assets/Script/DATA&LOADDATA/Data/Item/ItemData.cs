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
public class InteractOjectData
{
    public int objectId;
    public Vector3 position;
    public bool canInteract;
    public InteractOjectData(int objectId, Vector3 position, bool canInteract)
    {
        this.objectId = objectId;
        this.position = position;
        this.canInteract = canInteract;
    }
}
[System.Serializable]
public class AllItems
{
    public string sceneID;
    public List<ItemData> allItemsData = new List<ItemData>();
    public List<InteractOjectData> allInteractData = new();
}


