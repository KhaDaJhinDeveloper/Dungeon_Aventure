using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BaseButton : MonoBehaviour
{
    protected Button button;
    protected Vector3 originalPos;
    protected Vector3 originalScale;
    public Button Button { get => button; set => button = value; }

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        this.originalPos = transform.localPosition;
        this.originalScale = transform.localScale;
        AddOnClickEvent();
    }
    protected virtual void AddOnClickEvent()
    {
        button.onClick.AddListener(OnClick);
    }    
    protected virtual void OnClick()
    {
        transform.DOKill();
        transform.localScale = originalScale;
        transform.DOPunchScale(originalScale * 0.1f, 0.3f, 5, 0.5f).SetUpdate(true);
    }    
    protected virtual void OnDestroy()
    {
        transform.DOKill();
    }
}
