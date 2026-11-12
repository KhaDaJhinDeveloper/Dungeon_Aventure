using UnityEngine;
using UnityEngine.UI;

public class CraftingView : MonoBehaviour
{
    [SerializeField] private InventorySlotUI[] slotMarterialUI;
    public InventorySlotUI slotResultUI;
    void Start()
    {
        EventManager.Instance.Subscribe<int, InventorySlotData>(NameEvent.Event_LoadSlotsMaterialUI, LoadSlotsMaterialUI);
        EventManager.Instance.Subscribe<InventorySlotData>(NameEvent.Event_LoadSlotResultlUI, LoadSlotResultlUI);
    }
    void LoadSlotsMaterialUI(int index, InventorySlotData data)
    {
        if (index > this.slotMarterialUI.Length) return;
        var slotUI = this.slotMarterialUI[index];
        slotUI.UpdateUI(data);
    }
    void LoadSlotResultlUI(InventorySlotData data) => this.slotResultUI.UpdateUI(data);
    private void OnDestroy()
    {
        EventManager.Instance?.Unsubscribe<int, InventorySlotData>(NameEvent.Event_LoadSlotsMaterialUI, LoadSlotsMaterialUI);
        EventManager.Instance?.Unsubscribe<InventorySlotData>(NameEvent.Event_LoadSlotResultlUI, LoadSlotResultlUI);
    }
}
