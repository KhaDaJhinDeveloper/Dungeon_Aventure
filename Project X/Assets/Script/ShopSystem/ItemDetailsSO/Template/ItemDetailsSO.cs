using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/ItemDetails")]
public class ItemDetailsSO : ScriptableObject
{
    [Header("VisualItem")]
    public Sprite iconItem;
    [Header("DataItem")]
    public string nameItem;
    public ItemType itemType;
    public KeyPool keyPool;
    public int buyPrice;
    [TextArea] public string description;
}

[CreateAssetMenu(menuName = "Items/ItemDetailsDataBase")]
public class ItemDetailsDataBase : ScriptableObject
{
    public ItemDetailsSO[] itemDetails;
}

[CreateAssetMenu(menuName = "Items/ItemDetailShop")]
public class ItemDetailsShop : ScriptableObject
{
    public string shopID;
    [Range(1, 6)] public int maxSlot;
    public ItemDetailsSO[] itemList;
}

