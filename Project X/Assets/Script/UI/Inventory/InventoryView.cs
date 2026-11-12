using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventorySlotUI[] slotsUI;
    [SerializeField] private Image panel;
    void Start()
    {
        EventManager.Instance.Subscribe<int, InventorySlotData>(NameEvent.Event_InventoryView_OnSlotChanged, OnSlotChanged);
        EventManager.Instance.Subscribe<bool>(NameEvent.Event_InventoryView_SetPanel, SetPanel);
        this.panel.enabled = false;
    }

    public void OnSlotChanged(int index, InventorySlotData data)
    {
        if(index >= 0 && index < slotsUI.Length)
            this.slotsUI[index].UpdateUI(data);
    }
    void SetPanel(bool isActive) => this.panel.enabled =isActive;
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe<int, InventorySlotData>(NameEvent.Event_InventoryView_OnSlotChanged, OnSlotChanged);
        EventManager.Instance?.Unsubscribe<bool>(NameEvent.Event_InventoryView_SetPanel, SetPanel);
    }
}
