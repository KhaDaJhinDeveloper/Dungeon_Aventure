using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour, IPointerEnterHandler
{
    protected Button button;
    protected Vector3 originalPos;
    protected Vector3 originalScale;
    public Button Button { get => button; set => button = value; }

    protected virtual void Start()
    {
        button = GetComponent<Button>();
        this.originalPos = transform.localPosition;
        this.originalScale = transform.localScale;
        AddOnClickEvent();
    }
    protected virtual void AddOnClickEvent()
    {
        button.onClick.AddListener(OnClick);
    }    
    protected virtual void OnClick()
    {
        transform.DOKill();
        transform.localScale = originalScale;
        transform.DOPunchScale(originalScale * 0.1f, 0.3f, 5, 0.5f).SetUpdate(true);
        SoundManager.Instance?.PlayMusicSFX(SoundID.SFX_PressButton);
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance?.PlayMusicSFX(SoundID.SFX_HoverButton);
    }
    protected virtual void OnDestroy()
    {
        transform.DOKill();
    }
}
