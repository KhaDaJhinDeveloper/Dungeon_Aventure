using UnityEngine.SceneManagement;

public class ButtonRestart : BaseButton
{
    protected override void OnClick()
    {
        base.OnClick();
        TimeManager.TimeResume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);      
    }
}
