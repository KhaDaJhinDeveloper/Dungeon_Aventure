using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    }    
}
