using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class ButtonReturnMainMenu : BaseButton, IPointerEnterHandler
{
    protected override void OnClick()
    {
        base.OnClick();
        TimeManager.TimeResume();
        SceneManager.LoadScene("MainMenu");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
