using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler/*, IVirtualItemView<InventorySlotData>*/
{
    [SerializeField] private Image imageItem;
    [SerializeField] private Sprite imageDefault;
    [SerializeField] private TextMeshProUGUI textQuantity;
    private Canvas slotCanvas;
    private GraphicRaycaster slotRaycaster;
    private int raisedSortingOrder = 5;
    private bool isFull;
    public int indexSlot { get; private set; }
    #region DOTChildButtons
    [SerializeField] private RectTransform[] childButtons; 
    [SerializeField] private float offsetX ;   
    [SerializeField] private float spacingY ;    
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private float stagger = 0.05f;   
    private Ease easeOpen = Ease.OutBack;
    private Ease easeClose = Ease.InBack;
    private bool isOpen;
    #endregion
    private Sequence seq;
    private void Start()
    {
        foreach (var c in childButtons)
        {
            c.anchoredPosition = Vector2.zero;
            c.localScale = Vector3.zero;
            c.gameObject.SetActive(false);
        }
    }
    public void UpdateUI(InventorySlotData data)
    {
        if (data == null || data.IsEmpty)
        {
            this.imageItem.sprite = this.imageDefault;
            this.textQuantity.text = "";
            Close();
            SetEmpty();
            return;
        }
        this.imageItem.sprite = ItemDatabase.Instance.GetItemSprite(data.itemName);
        this.textQuantity.text = data.quantity > 1 ?"x" + data.quantity.ToString() : "";
        this.indexSlot = data.index;
        this.isFull = data.quantity >= 1 ? SetFull() : SetEmpty();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!this.isFull) return;
        if (!isOpen)
            Open();
        else Close();
        SoundManager.Instance.PlayMusicSFX(SoundID.SFX_Interface_InventorySlotItem);
    }
    public void Open()
    {
        isOpen = true;
        seq?.Kill();
        seq = DOTween.Sequence().SetUpdate(true);
        SetRaised(true);
        for (int i = 0; i < childButtons.Length; i++)
        {
            var c = childButtons[i];
            c.gameObject.SetActive(true);
            c.anchoredPosition = Vector2.zero;
            c.localScale = Vector3.zero;

            Vector2 target = new Vector2(offsetX, -i * spacingY);
            float delay = i * stagger;

            seq.Insert(delay, c.DOAnchorPos(target, duration).SetEase(easeOpen));
            seq.Insert(delay, c.DOScale(1f, duration).SetEase(easeOpen));
        }
        EventManager.Instance.TriggerEvent(NameEvent.Event_InventoryView_SetPanel, true);
    }
    public void Close()
    {
        isOpen = false;
        seq?.Kill();
        seq = DOTween.Sequence().SetUpdate(true);

        for (int i = 0; i < childButtons.Length; i++)
        {
            var c = childButtons[i];
            float delay = (childButtons.Length - 1 - i) * stagger; 

            seq.Insert(delay, c.DOAnchorPos(Vector2.zero, duration).SetEase(easeClose));
            seq.Insert(delay, c.DOScale(0f, duration).SetEase(easeClose));
        }

        seq.OnComplete(() =>
        {
            foreach (var c in childButtons) c.gameObject.SetActive(false);
            SetRaised(false);
            EventManager.Instance.TriggerEvent(NameEvent.Event_InventoryView_SetPanel, false);
        });
    }
    private void SetRaised(bool isRaised)
    {
        if(isRaised)
        {
            if (this.slotCanvas != null) return;
            this.slotCanvas = gameObject.AddComponent<Canvas>();
            this.slotCanvas.overrideSorting = true;
            this.slotCanvas.sortingLayerName = "UI";
            this.slotCanvas.sortingOrder = this.raisedSortingOrder;
            this.slotRaycaster = gameObject.AddComponent<GraphicRaycaster>();
        }
        else
        {
            if (this.slotRaycaster != null) Destroy(this.slotRaycaster);
            if (this.slotCanvas != null) Destroy(this.slotCanvas);
            this.slotRaycaster = null;
            this.slotCanvas = null;
        }
    }

    public bool SetFull() => this.isFull = true;
    public bool SetEmpty()=> this.isFull = false;
    private void OnDisable()
    {
        if (this.isOpen) Close();
    }
    private void OnDestroy()
    {
        this.seq?.Kill();
    }
}
