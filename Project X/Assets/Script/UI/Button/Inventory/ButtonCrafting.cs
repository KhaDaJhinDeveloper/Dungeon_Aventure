using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCrafting : BaseButton, IPointerEnterHandler
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
        this.craftingItem.Crafting();
        EventManager.OP_EventManager.TriggerEvent("LoadCraftingeUI");
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
