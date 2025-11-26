using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonReset : BaseButton, IPointerEnterHandler
{
    private CraftingItem craftingItem;
    protected override void Start()
    {
        base.Start();
        this.craftingItem = GameObject.FindWithTag(TagManager.TAG_UI).GetComponentInChildren<CraftingItem>();
    }
    protected override void OnClick()
    {
        base.OnClick();
        this.craftingItem.ResetAllMaterial();
        EventManager.OP_EventManager.TriggerEvent("LoadUIDefault");
        EventManager.OP_EventManager.TriggerEvent<string>("LoadCraftingReportText", "Returned raw materials to inventory");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
