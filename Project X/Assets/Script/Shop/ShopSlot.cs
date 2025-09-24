using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    public int priceItem;
    public string describe;
    public string nameItem;
    private ShopManager shopManager;
    void Start()
    {
        this.shopManager = GameObject.FindFirstObjectByType<ShopManager>();
    }
    void Update()
    {
        
    }
}
