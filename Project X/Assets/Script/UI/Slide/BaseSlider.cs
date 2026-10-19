using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSlider : MonoBehaviour, ILoadUI
{
    protected Image sliderBar;
    protected Slider slider;
    protected virtual void Start()
    {
        sliderBar = GetComponent<Image>();
    }
    public virtual void Load()
    {

    }
    protected virtual void AddOnSliderValueChange()
    {
        if(this.slider == null) this.slider = GetComponent<Slider>();
        this.slider.onValueChanged.AddListener(LoadSlider);
    }
    protected virtual void LoadSlider(float value)
    {
    }
}
