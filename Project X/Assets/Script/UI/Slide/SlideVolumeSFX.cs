
using UnityEngine.UI;

public class SlideVolumeSFX : BaseSlider
{
    protected override void Start()
    {
        base.Start();
        this.slider = GetComponent<Slider>();
        this.slider.value = SoundManager.Instance.volumeSFX;
        AddOnSliderValueChange();
    }
    protected override void LoadSlider(float value)
    {
        base.LoadSlider(value);
        SoundManager.Instance.volumeSFX = value;
        SoundManager.Instance.SetVolumeSFX(value);
    }
}
