using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    [SerializeField] private ShopSlotUI[] slotsUI;
    void Start()
    {
        EventManager.Instance.Subscribe<int>(NameEvent.Event_ShopView_RefreshView, RefreshView);
        EventManager.Instance.Subscribe<int, ShopSlotData>(NameEvent.Event_ShopView_UpdateSlotView, UpdateSlotView);
    }
    public void RefreshView(int maxSlot)
    {
        for (int i = 0; i < this.slotsUI.Length; i++)
        {
            this.slotsUI[i].SetIndex(i);
            bool active = i < maxSlot;
            this.slotsUI[i].gameObject.SetActive(active); 
        }               
    }
    public void UpdateSlotView(int index, ShopSlotData data)
    {
        if (index > this.slotsUI.Length) return;
        this.slotsUI[index].UpdateUI(data);
    }
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe<int>(NameEvent.Event_ShopView_RefreshView, RefreshView);
        EventManager.Instance?.Unsubscribe<int, ShopSlotData>(NameEvent.Event_ShopView_UpdateSlotView, UpdateSlotView);
    }
}
