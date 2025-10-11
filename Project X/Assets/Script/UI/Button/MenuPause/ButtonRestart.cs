using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonRestart : BaseButton, IPointerEnterHandler
{
    protected override void OnClick()
    {
        base.OnClick();
        TimeManager.TimeResume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);      
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
}
