using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonNextDialog : BaseButton, IPointerEnterHandler
{
    private DialogManager dialogManager;
    protected override void Start()
    {
        base.Start();
        this.dialogManager = GameObject.FindFirstObjectByType<DialogManager>();
    }
    protected override void OnClick()
    {
        base.OnClick();
        this.dialogManager.OnNextButtonClicked();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
