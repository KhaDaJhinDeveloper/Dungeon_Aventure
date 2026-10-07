using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDataManager : Singleton<ItemDataManager>,IDataManager
{
    #region FILE_ITEM_DATA
    private const string FILE_ITEM_DATA = "ItemData.json";
    #endregion
    private List<GameObject> itemsList = new List<GameObject>();
    private AllItems allItems;
    public void SaveData()
    {
        this.allItems = new AllItems();
        this.allItems.sceneID = SceneExtensions.GetCurrentSceneName();
        this.allItems.allItemsData = GetItemsData();
        this.allItems.allInteractData = GetInteractData();
        JsonFileUtility.SaveToJson(this.allItems, FILE_ITEM_DATA);
    }    
    public void LoadData()
    {
        AllItems allItems = JsonFileUtility.LoadFromJson<AllItems>(FILE_ITEM_DATA);
        if (allItems == null) return;
        foreach(ItemData data in allItems.allItemsData)
        {
            KeyPool nameItem =(KeyPool)data.itemId;
            GameObject objItem = ObjectPooling.ObjectPooling_Instance.GetPool(nameItem);
            objItem.transform.position = data.position;
        }
        foreach(InteractOjectData objData in allItems.allInteractData)
        {
            KeyPool interactPool = (KeyPool)objData.objectId;
            GameObject interactObj = ObjectPooling.Instance.GetPool(interactPool);
            interactObj.transform.position = objData.position;
            BaseInteraction intercatComponent = interactObj.GetComponent<BaseInteraction>();
            intercatComponent.OnLoadData(objData.canInteract);
        }
    }
    public void DeleteData()
    {
        JsonFileUtility.DeleteJsonFile(FILE_ITEM_DATA);
        AllItems allItems = new AllItems();
    }
    private List<GameObject> GetItemsList()
    {
        List<GameObject> itemsList = new List<GameObject>();
        GameObject[] item = GameObject.FindGameObjectsWithTag(TagManager.TAG_ITEM);
        foreach (GameObject itemObj in item)
        {
            if (!itemObj.activeInHierarchy) continue;
            else itemsList.Add(itemObj);
        }
        return itemsList;
    }
    private List<ItemData> GetItemsData()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        this.itemsList = GetItemsList().ToList();
        foreach (GameObject itemObj in this.itemsList)
        {
            ItemKey itemKey = itemObj.GetComponent<ItemKey>();
            if(itemKey != null)
            {
                int itemId = (int)itemKey.keyPool;
                Vector3 itemPos = itemObj.transform.position;
                itemDatas.Add(new ItemData(itemId, itemPos));
            }
        }
        return itemDatas;
    }
    private List<InteractOjectData> GetInteractData()
    {
        List<InteractOjectData> data = new();
        GameObject[] listObjt = GameObject.FindGameObjectsWithTag(TagManager.TAG_INTERACTOBECJT);
        foreach(var obj in listObjt)
        {
            BaseInteraction intercatComponent = obj.GetComponent<BaseInteraction>();
            if (obj.TryGetComponent<ItemKey>(out ItemKey key))
                data.Add(new InteractOjectData((int)key.keyPool, obj.transform.position, intercatComponent.canInteract));
        }
        return data;
    }
    public bool HasMapData(string mapID)
    {
        if (!HasData()) return false;
        AllItems data = JsonFileUtility.LoadFromJson<AllItems>(FILE_ITEM_DATA);
        if (string.IsNullOrEmpty(data.sceneID))
            return false;
        else return data.sceneID == mapID;
    }
    public bool HasData()
    {
        return JsonFileUtility.JsonFileExists(FILE_ITEM_DATA);
    }
}
