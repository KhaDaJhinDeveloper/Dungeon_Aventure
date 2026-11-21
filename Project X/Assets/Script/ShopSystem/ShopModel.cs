using UnityEngine;

public class ShopModel
{
    public const int MAX_SLOT = 6;
    public ShopSlotData[] slots { get; private set; }
    public float priceResonance { get; private set; }  = 0.0f;
    public void Initialize()
    {
        this.slots = new ShopSlotData[MAX_SLOT];
        for (int i = 0; i < slots.Length; i++)
        {
            this.slots[i] = new ShopSlotData();
            this.slots[i].SetIndex(i);
        }
    }
    public void LoadStore(int maxSlot ,ItemDetailsShop itemDetailsShop, float scalePrice)
    {
        if (maxSlot > this.slots.Length) return;
        for (int i = 0;i < maxSlot ;i++)
        {
            int index = Random.Range(0, itemDetailsShop.itemList.Length);
            slots[i].SetData(itemDetailsShop.itemList[index], scalePrice, this.priceResonance);
            EventManager.Instance.TriggerEvent(NameEvent.Event_ShopView_UpdateSlotView, i, this.slots[i]);
        }
        EventManager.Instance.TriggerEvent(NameEvent.Event_ShopView_RefreshView, maxSlot);
    }
    public void RefreshStore(int maxSlot, ItemDetailsShop itemDetailsShop, float scalePrice)
    {
        if (maxSlot > this.slots.Length) return;
        this.priceResonance += 0.05f;
        for (int i = 0; i < maxSlot; i++)
        {
            int index = Random.Range(0, itemDetailsShop.itemList.Length);
            slots[i].SetData(itemDetailsShop.itemList[index], scalePrice, this.priceResonance);
            EventManager.Instance.TriggerEvent(NameEvent.Event_ShopView_UpdateSlotView, i, this.slots[i]);
        }
        EventManager.Instance.TriggerEvent(NameEvent.Event_ShopView_RefreshView, maxSlot);
    }
    public void Buy(int index)
    {
        var slot = this.slots[index];
        RemoveShopSlot(slot);
    }
    public void SetPriceResonance(float priceResonance)=> this.priceResonance = priceResonance;
    public void RemoveShopSlot(ShopSlotData slot)
    {
        slot.Clear();
        EventManager.Instance.TriggerEvent(NameEvent.Event_ShopView_UpdateSlotView, slot.index, this.slots[slot.index]);
    }
}
