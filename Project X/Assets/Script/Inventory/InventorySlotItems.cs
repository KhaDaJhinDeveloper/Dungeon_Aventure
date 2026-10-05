using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotItems : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image imageItem;
    [SerializeField] private GameObject option;
    [SerializeField] private GameObject slotSelected;
    [SerializeField] private Sprite imageDefault;
    private KeyPool key;
    private ItemType type;
    private bool isSelected;
    private string nameItem; 
    private bool isFull = false;
    //-------------------------------------------------
    private Transform posDrop;
    private InventoryManager inventoryManager;
    private CraftingItem craftingItem;
    public bool IsFull { get => this.isFull; set => this.isFull = value; }
    public string NameItem { get => nameItem; set => nameItem = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }
    public GameObject SlotSelected { get => slotSelected; set => slotSelected = value; }
    public GameObject Option { get => option; set => option = value; }
    public Image ImageItem { get => imageItem; set => imageItem = value; }
    public ItemType Type { get => type; set => type = value; }
    public KeyPool Key { get => this.key; }

    void Start()
    {
        this.inventoryManager = GameObject.FindWithTag(TagManager.TAG_UI).GetComponentInChildren<InventoryManager>();
        this.craftingItem = GameObject.FindWithTag(TagManager.TAG_UI).GetComponentInChildren<CraftingItem>();
        this.posDrop = GameObject.FindWithTag(TagManager.TAG_DROP_POSITION).transform;
    }
    public void AddItem(Sprite imageItem, string nameItem, ItemType type, KeyPool key)
    {
        this.imageItem.sprite = imageItem;
        this.nameItem = nameItem;
        this.type = type;
        this.key = key;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }
    private void OnLeftClick()
    {
        if(!this.isSelected)
        {
            if (this.isFull)
            {
                this.inventoryManager.DeselectedAllSlots();
                this.option.SetActive(true);
                this.slotSelected.SetActive(true);
                this.isSelected = true;
            }
            else return;//Add Sound Effect
        }    
        else
        {
            if (this.type == ItemType.Usable)
            {
                EventManager.OP_EventManager.TriggerEvent<string>("LoadSlotItemReportText", "Used item");
                this.inventoryManager.UseItem(this.nameItem);
                EmptySlot();
            }
            else if (this.type == ItemType.Material)
            {
                EventManager.OP_EventManager.TriggerEvent<string>("LoadSlotItemReportText", "This is material, not usable");
            }
        }    
    }
    public void EmptySlot()
    {
        this.imageItem.sprite = this.imageDefault;
        this.isSelected = false;
        if (this.option != null) this.option.SetActive(false);
        if (this.slotSelected != null) this.slotSelected.SetActive(false);
        this.isFull = false;
        this.nameItem = null;
    }
    public void DropItemSlot()
    {
        GameObject objItem = ObjectPooling.ObjectPooling_Instance.GetPool(this.key);
        objItem.transform.position = this.posDrop.position;
        EmptySlot();
    }
    public void AddMaterial()
    {
        this.craftingItem.AddMaterial(this);
    }
}
