using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    public Sprite imageItem;
    public string nameItem;
    public int priceItem;
    public ItemType typeItem;
    [TextArea] public string describe;
    public GameObject handleSelected;
    public bool isSelected = false;
    private ShopManager shopManager;
    void Start()
    {
        this.shopManager = GameObject.FindFirstObjectByType<ShopManager>();
        LoadDataItem();
        this.handleSelected.SetActive(false);
    }
    void Update()
    {
        this.handleSelected.SetActive(this.isSelected);
    }
    void Additem()
    {

    }    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }
    public void LoadDataItem()
    {
        int dataIndex = Random.Range(0, this.shopManager.itemsDetails.Count);
        ItemDetailsSO currentslot = this.shopManager.itemsDetails[dataIndex];
        this.imageItem = currentslot.imageItem;
        this.nameItem = currentslot.nameItem;
        this.priceItem = currentslot.priceItem;
        this.describe = currentslot.describe;
        this.typeItem = currentslot.typeItem;
    }
    void OnLeftClick()
    {
        this.shopManager.SelectedSlot(this);
    }
}
