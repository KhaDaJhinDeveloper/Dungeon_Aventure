using UnityEngine;
[CreateAssetMenu(menuName = "Items/ItemDetails")]
public class ItemDetailsSO : ScriptableObject
{
    public Sprite imageItem;
    public string nameItem;
    public int priceItem;
    public ItemType typeItem;
    [TextArea] public string describe;
}
