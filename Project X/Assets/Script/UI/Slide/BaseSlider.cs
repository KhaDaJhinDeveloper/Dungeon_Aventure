using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSlider : MonoBehaviour, ILoadUI
{
    protected Image sliderBar;
    protected virtual void Start()
    {
        sliderBar = GetComponent<Image>();
    }
    public virtual void Load()
    {

    }
}
