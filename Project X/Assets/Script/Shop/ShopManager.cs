using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Dictionary<string, int> priceItems;
    [SerializeField] private GameObject shop;
    void Start()
    {
        this.priceItems = new Dictionary<string, int>()
        {
            {"Itemm1",2999 },
            {"Item2",2999 }
        };
        CloseShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenShop() => this.shop.SetActive(true);
    public void CloseShop() => this.shop.SetActive(false);
}
