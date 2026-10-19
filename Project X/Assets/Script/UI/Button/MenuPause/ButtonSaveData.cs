using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonSaveData : BaseButton, IPointerEnterHandler
{
    protected override void OnClick()
    {
        base.OnClick();
        GameSaveManager.Instance.SaveAllDataLocal();
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
