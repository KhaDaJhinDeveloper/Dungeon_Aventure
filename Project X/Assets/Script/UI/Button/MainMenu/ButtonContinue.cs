
public class ButtonContinue : BaseButton
{
    protected override void Start()
    {
        base.Start();
        this.button.interactable = GameSaveManager.Instance.HasData();
    }
    protected override void OnClick()
    {
        GameControl.Instance.Continue();
    }
}
