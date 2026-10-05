using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLoadLevel : BaseButton
{
    [SerializeField] private NameScene nameScene;
    protected override void OnClick()
    {
        base.OnClick();
        TransitionScene.Instance.PlayTransition
        (
            () => SceneManager.LoadScene(this.nameScene.ToString())
        );      
    }
}
