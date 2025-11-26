using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    public List<ItemDetailsSO>  itemsDetails = new List<ItemDetailsSO>();
    public ShopSlot currentSlot = null;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject bacckGround;
    [SerializeField] private Transform startPos;
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
        this.shopUI.transform.DOKill();      
        this.bacckGround.SetActive(true);
        this.shopUI.SetActive(true);
        this.shopUI.transform.position = this.startPos.transform.position;
        this.shopUI.transform.DOMoveX(this.shopUI.transform.position.x - 11.5f, 0.3f).SetDelay(0.1f).SetUpdate(true);
        TimeManager.TimePause();
    }     
    public void CloseShop()
    {
        this.shopUI.transform.DOKill();      
        this.shopUI.transform.DOMoveX(this.shopUI.transform.position.x + 11.5f, 0.5f).SetDelay(0.1f).SetUpdate(true).OnComplete(()=> { this.bacckGround.SetActive(false); 
                                                                                                                                       this.shopUI.SetActive(false); 
                                                                                                                                       TimeManager.TimeResume(); });      
    }     
    private void OnDestroy()
    {
        this.shopUI.transform.DOKill();
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_OpenShop, OpenShop);
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_CloseShop, CloseShop);
    }
}
