using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private ItemType type;
    [SerializeField] private string nameitem;
    [SerializeField] private Sprite imageItem;
    private InventoryManager inventoryManager;
    private SpriteRenderer spriteRenderer;
    public string Nameitem { get => nameitem; }
    public Sprite ImageItem { get => imageItem; }
    public ItemType Type { get => type; set => type = value; }

    private void Start()
    {

        this.inventoryManager = GameObject.FindWithTag(TagManager.TAG_INVENTORY_MANAGER).GetComponent<InventoryManager>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.LoadIndexItem();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.inventoryManager.AddItem(this.imageItem, this.nameitem, this.type);
            ObjectPooling.ObjectPooling_Instance.ReturnToPool(this.nameitem, this.gameObject);
        }
    }
    private void LoadIndexItem()
    {
        this.nameitem = KeyClean.CleanKey(this.name);
        this.imageItem = this.spriteRenderer.sprite;
    }
}
