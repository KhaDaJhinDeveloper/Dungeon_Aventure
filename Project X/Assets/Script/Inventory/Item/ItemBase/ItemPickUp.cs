using UnityEngine;


public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private KeyPool keyPool;
    [SerializeField] private ItemType type;
    [SerializeField] private Sprite imageItem;
    private string nameitem;
    private InventoryManager inventoryManager;
    private SpriteRenderer spriteRenderer;
    public KeyPool Key_Pool { get => keyPool; }
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
           if(!this.inventoryManager.IsFullSlot())
            {
                this.inventoryManager.AddItem(this.imageItem, this.nameitem, this.type, this.keyPool);
                ObjectPooling.ObjectPooling_Instance.ReturnToPool(this.keyPool, this.gameObject);
            }    
        }
    }
    private void LoadIndexItem()
    {
        this.nameitem = KeyClean.CleanKey(this.name);
        this.imageItem = this.spriteRenderer.sprite;
    }
}
