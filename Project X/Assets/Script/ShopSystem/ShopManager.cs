using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShopManager : MonoBehaviour
{
    public List<ItemDetailsSO>  itemsDetails = new List<ItemDetailsSO>();
    public ShopSlot currentSlot = null;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject bacckGround;
    private Vector3 startPos;
    void Start()
    {
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_OpenShop, OpenShop);
        EventManager.OP_EventManager.Subscribe(NameEvent.Event_CloseShop, CloseShop);
        this.startPos = this.shop.transform.position;
        CloseShop();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            OpenShop();
        if (Input.GetKeyDown(KeyCode.Z))  
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
        this.shop.transform.DOKill();      
        this.bacckGround.SetActive(true);
        this.shop.SetActive(true);        
        this.shop.transform.position = this.startPos;
        this.shop.transform.DOMoveX(this.shop.transform.position.x - 11.5f, 0.5f).SetDelay(0.1f).SetUpdate(true);
        TimeManager.TimePause();
    }     
    public void CloseShop()
    {
        this.shop.transform.DOKill();      
        this.shop.transform.DOMoveX(this.shop.transform.position.x + 11.5f, 0.5f).SetDelay(0.1f).SetUpdate(true).OnComplete(()=> { this.bacckGround.SetActive(false); this.shop.SetActive(false); TimeManager.TimeResume(); });      
    }     
    private void OnDestroy()
    {
        this.shop.transform.DOKill();
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_OpenShop, OpenShop);
        EventManager.OP_EventManager.Unsubscribe(NameEvent.Event_CloseShop, CloseShop);
    }
}
