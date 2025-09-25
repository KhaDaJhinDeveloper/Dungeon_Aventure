using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<ItemDetailsSO>  itemsDetails = new List<ItemDetailsSO>();
    public ShopSlot currentSlot = null;
    [SerializeField] private GameObject shop;
    void Start()
    {
        CloseShop();
    }
    public void SelectedSlot(ShopSlot slot)
    {
        if (this.currentSlot != null)
            this.currentSlot.isSelected = false; 

        this.currentSlot = slot;
        this.currentSlot.isSelected = true;         
    }    
    public void OpenShop() => this.shop.SetActive(true);
    public void CloseShop() => this.shop.SetActive(false);
}
