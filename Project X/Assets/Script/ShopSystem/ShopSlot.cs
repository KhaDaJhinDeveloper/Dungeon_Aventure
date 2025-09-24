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
        this.handleSelected.SetActive(false);
    }
    void Update()
    {
        this.handleSelected.SetActive(this.isSelected);
    }
    public void AddItem(Sprite sprite, string nameItem, int priceItem, string describe, ItemType type)
    {
        this.imageItem = sprite;
        this.nameItem = nameItem;
        this.priceItem = priceItem;
        this.describe = describe;
        this.typeItem = type;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
    }
    void OnLeftClick()
    {
        this.shopManager.SelectedSlot(this);
    }
}
