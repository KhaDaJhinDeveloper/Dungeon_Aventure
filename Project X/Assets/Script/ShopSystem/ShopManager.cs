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
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_OpenShop, OpenShop);
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_CloseShop, CloseShop);
        CloseShop();
    }
    public void SelectedSlot(ShopSlot slot)
    {
        if (this.currentSlot != null)
            this.currentSlot.isSelected = false; 

        this.currentSlot = slot;
        this.currentSlot.isSelected = true;         
    }    
    public void OpenShop()
    {
        this.shop.SetActive(true);
        TimeManager.TimePause();
    }     
    public void CloseShop()
    {
        this.shop.SetActive(false);
        TimeManager.TimeResume();
    }     
    private void OnDestroy()
    {
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_OpenShop, OpenShop);
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_CloseShop, CloseShop);
    }
}
