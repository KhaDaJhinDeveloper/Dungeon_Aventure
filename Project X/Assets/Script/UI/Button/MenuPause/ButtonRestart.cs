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
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
