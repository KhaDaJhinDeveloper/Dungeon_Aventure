
public class ButtonDeleteData : BaseButton
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnClick()
    {
        DebugLogger.Log("Delete Data"); 
        GameSaveManager.Instance.DeleteAllDataLocal();
    }
}
