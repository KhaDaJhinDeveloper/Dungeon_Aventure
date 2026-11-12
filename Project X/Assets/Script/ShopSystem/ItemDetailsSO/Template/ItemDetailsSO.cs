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
