using UnityEngine.UI;
public class SlideVolumeBG : BaseSlider
{
    protected override void Start()
    {
        base.Start();
        this.slider = GetComponent<Slider>();
        this.slider.value = SoundManager.Instance.volumeBG;
        AddOnSliderValueChange();
    }
    protected override void LoadSlider(float value)
    {
        base.LoadSlider(value);
        SoundManager.Instance.volumeBG = value;
        SoundManager.Instance.SetVolumeBG(value);
    }
}
