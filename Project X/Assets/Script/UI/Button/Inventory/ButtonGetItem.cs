using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonGetItem : BaseButton, IPointerEnterHandler
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
        this.craftingItem.GetItemcomplete();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
