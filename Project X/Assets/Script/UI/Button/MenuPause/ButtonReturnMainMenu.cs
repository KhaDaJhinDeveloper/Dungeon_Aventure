using DG.Tweening;
using UnityEngine.EventSystems;
public class ButtonReturnMainMenu : BaseButton, IPointerEnterHandler
{
    protected override void OnClick()
    {
        base.OnClick();
        GameControl.Instance?.ReturnMainMenu();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
