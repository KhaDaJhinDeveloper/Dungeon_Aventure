using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    private InventoryManager inventoryManager;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject bacckGround;
    [SerializeField] private Transform startPos;
    private CoinManager coinManager;
    [SerializeField] private ItemDetailsDataBase itemDetailsDataBase;
    private bool existItem;
    private ItemDetailsShop currentShopDetails;
    public ShopModel shopModel { get; private set; } = new();
    private ShopRunTime currentShopData;
    private Dictionary<string, ShopRunTime> shopRunTimeDatabase = new();
    void Start()
    {
        this.inventoryManager = FindFirstObjectByType<InventoryManager>();
        this.coinManager = FindFirstObjectByType<CoinManager>();
        EventManager.Instance.Subscribe(NameEvent.Event_OpenShopUI, OpenShop);
        EventManager.Instance.Subscribe(NameEvent.Event_CloseShopUI, CloseShop);
        EventManager.Instance.Subscribe<ItemDetailsShop>(NameEvent.Event_ShopSystem_GetItemDetails, GetItemDetails);
        EventManager.Instance.Subscribe<int>(NameEvent.Event_ShopSystem_BuyItem, BuyItem);
        EventManager.Instance.Subscribe(NameEvent.Event_ShopSystem_RefreshShop, RefreshShop);
        CloseShop();
        this.shopModel.Initialize();
    }
    public void GetItemDetails(ItemDetailsShop itemDetails)
    {
        string shopID = itemDetails.shopID;
        this.existItem = (this.currentShopDetails != null && this.currentShopDetails == itemDetails) ? true : false;
        if(!this.shopRunTimeDatabase.TryGetValue(shopID, out ShopRunTime value))
        {
            this.currentShopData = new ShopRunTime
            {
                currentRefresh = 0,
                maxRefresh = 5,
                priceResonance = 0
            };
        }
        else
        {
            this.currentShopData = new ShopRunTime
            {
                currentRefresh = value.currentRefresh,
                maxRefresh = value.maxRefresh,
                priceResonance = value.priceResonance
            };
        }
        this.shopModel.SetPriceResonance(this.currentShopData.priceResonance);
        this.currentShopDetails = itemDetails;
        int maxSlot = currentShopDetails.maxSlot;
        float scalePrice = GameControl.Instance.ScalePrice;
        if (this.existItem) return;
        this.shopModel.LoadStore(maxSlot,this.currentShopDetails, scalePrice);
        this.existItem = true;
    }
    public void BuyItem(int index)
    {
        if (index < 0 || index >= this.shopModel.slots.Length) return;

        var slot = this.shopModel.slots[index];
        if (slot == null || slot.IsEmpty) return;

        if (this.coinManager.CoinAmount < slot.price)
        {
            Debug.Log("Not enough coin");
            return;
        }
        string itemName = slot.itemName;
        ItemType itemType = slot.type;
        KeyPool keyPool = slot.key;
        bool isAdd = this.inventoryManager.TryPickUpItem(itemName, itemType, keyPool);
        if (!isAdd)
        {
            Debug.Log("Full Slot");
            return;
        }
        this.coinManager.SpendCoin(slot.price);
        this.shopModel.Buy(index);
    }
    public void RefreshShop()
    {
        if (this.currentShopData.IsOutOfRefresh) return;
        float scalePrice = GameControl.Instance.ScalePrice;
        this.shopModel.RefreshStore(this.currentShopDetails.maxSlot, this.currentShopDetails, scalePrice);
        this.currentShopData.currentRefresh++;
        this.existItem = true;
    }
    public void SaveRunTime()
    {
       if (this.currentShopData == null || this.currentShopDetails == null) return;
        string shopId = this.currentShopDetails.shopID;
        this.currentShopData.priceResonance = this.shopModel.priceResonance;
        this.shopRunTimeDatabase[shopId] = this.currentShopData;
    }    
    #region ShopUI
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
        SaveRunTime();
        this.shopUI.transform.DOKill();      
        this.shopUI.transform.DOMoveX(this.shopUI.transform.position.x + 11.5f, 0.5f).SetDelay(0.1f).SetUpdate(true).OnComplete(()=> { this.bacckGround.SetActive(false); 
                                                                                                                                       this.shopUI.SetActive(false); 
                                                                                                                                       TimeManager.TimeResume(); });      
    }
    #endregion
    private void OnDestroy()
    {
        this.shopUI.transform.DOKill();
        EventManager.Instance?.Unsubscribe(NameEvent.Event_OpenShopUI, OpenShop);
        EventManager.Instance?.Unsubscribe(NameEvent.Event_CloseShopUI, CloseShop);
        EventManager.Instance?.Unsubscribe<int>(NameEvent.Event_ShopSystem_BuyItem, BuyItem);
        EventManager.Instance?.Unsubscribe<ItemDetailsShop>(NameEvent.Event_ShopSystem_GetItemDetails, GetItemDetails);
        EventManager.Instance?.Unsubscribe(NameEvent.Event_ShopSystem_RefreshShop, RefreshShop);
    }
}
public class ShopRunTime
{
    public int currentRefresh = 0;
    public int maxRefresh = 5;
    public float priceResonance;
    public bool IsOutOfRefresh  => currentRefresh >= maxRefresh;
}
