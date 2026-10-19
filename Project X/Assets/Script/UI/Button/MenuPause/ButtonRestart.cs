using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonRestart : BaseButton, IPointerEnterHandler
{
    protected override void OnClick()
    {
        base.OnClick();
        TimeManager.TimeResume();
        GameControl.Instance.Restart(); 
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
