using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BaseButton : MonoBehaviour
{
    protected Button button;

    public Button Button { get => button; set => button = value; }

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        AddOnClickEvent();
    }
    protected virtual void AddOnClickEvent()
    {
        button.onClick.AddListener(OnClick);
    }    
    protected virtual void OnClick()
    {
        transform.DOKill();
        transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 5, 0.5f).SetUpdate(true);
    }    
    protected virtual void OnDestroy()
    {
        transform.DOKill();
    }
}
