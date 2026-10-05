using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class BtnDOTWEEN : BaseButton, IPointerEnterHandler
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    protected override void OnClick()
    {
        base.OnClick();
        //transform.DOShakePosition(0.3f, 10f, 20, 90, false, true);

        //transform.DOShakeRotation(0.3f, 10f, 10, 90, true);

        //transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 5, 0.5f);

        /*
         * Image img = GetComponent<Image>();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(img.DOFade(0.5f, 0.1f));
        sequence.Join(transform.DOScale(0.95f, 0.1f));
        sequence.Append(img.DOFade(1f, 0.1f));
        sequence.Join(transform.DOScale(1f, 0.1f));
        */

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOShakePosition(0.3f, 10f, 20, 90, false, true).SetUpdate(true);
    }
    protected override void OnDestroy()
    {
        transform.DOKill();
    }
}
