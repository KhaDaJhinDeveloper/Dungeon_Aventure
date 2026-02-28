using System.Collections.Generic;
using System.Linq;
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
        this.allItems.allItemsData = GetItemsData();
        JsonFileUtility.SaveToJson(this.allItems, FILE_ITEM_DATA);
    }    
    public void LoadData()
    {
        AllItems allItems = JsonFileUtility.LoadFromJson<AllItems>(FILE_ITEM_DATA);
        if (allItems == null) return;
        foreach(ItemData data in allItems.allItemsData)
        {
            string nameItem = data.nameItem;
            GameObject objItem = ObjectPooling.ObjectPooling_Instance.GetPool(nameItem);
            objItem.transform.position = data.position;
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
            string nameITem = KeyClean.CleanKey(itemObj.name);
            Vector3 itemPos = itemObj.transform.position;
            itemDatas.Add(new ItemData(nameITem, itemPos));
        }    
        return itemDatas;
    }
}
