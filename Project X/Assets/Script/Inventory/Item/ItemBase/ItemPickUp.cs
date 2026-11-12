using UnityEngine;


public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private KeyPool keyPool;
    [SerializeField] private ItemType type;
    [SerializeField] private Sprite imageItem;
    private string nameitem;
    private InventoryManager inventoryManager;
    public KeyPool Key_Pool { get => keyPool; }
    public Sprite ImageItem { get => imageItem; }
    public ItemType Type { get => type; }
    private bool isPickUp;

    private void Start()
    {
        this.inventoryManager = FindFirstObjectByType<InventoryManager>();
        this.LoadIndexItem();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.isPickUp) return;
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
           if(this.inventoryManager.TryPickUpItem(this.nameitem, this.type, this.keyPool))
            {
                this.isPickUp = true;
                SoundManager.Instance.PlayMusicSFX(SoundID.SFX_Pickup_Item, this.transform, 0);
                ObjectPooling.ObjectPooling_Instance.ReturnToPool(this.keyPool, this.gameObject);
            }    
        }
    }
    private void LoadIndexItem()
    {
        this.nameitem = KeyClean.CleanKey(this.name);
    }
    private void OnEnable()
    {
        this.isPickUp = false;
    }
}
