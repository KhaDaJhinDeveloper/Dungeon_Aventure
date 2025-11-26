using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonNormal : BaseButton, IPointerEnterHandler
{
    protected override void OnClick()
    {
        base.OnClick();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localPosition = this.originalPos;
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
