using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextPopupCoin : BaseText
{
    string namekey;
    protected override void Start()
    {
        base.Start();
        m_Text = GetComponentInChildren<TextMeshPro>();
    }
    public void Notification(Vector3 pos, int textinput)
    {
        StartCoroutine(Effect(pos, textinput));
    }
    IEnumerator Effect(Vector3 pos, int textinput)
    {
        m_Text.text ="Coin + " + textinput.ToString();
        this.transform.position = pos;

        float timer = 0f;
        float duration = 1f;
        float moveSpeed = 2f;

        while (timer < duration)
        {
            this.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        m_Text.text = "";
        ObjectPooling.ObjectPooling_Instance.ReturnToPool(KeyPool.KEY_VFX_TEXTPOPUPCOIN, this.gameObject);
    }
}
