using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    [SerializeField] private string nameitem;
    [SerializeField] private int amount;
    private CoinManager coinManager;
    public string Nameitem { get => nameitem; }
    public int Amount { get => amount; }
    private void Start()
    {
        this.nameitem = KeyClean.CleanKey(this.name);
        this.coinManager = GameObject.FindFirstObjectByType<CoinManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.coinManager.IncreaseCoinAmount(this.amount);
            StartCoroutine(Effect(this.transform.position, this.amount));
        }
    }
    IEnumerator Effect(Vector3 pos, int amount)
    {
        GameObject textShowCoin = ObjectPooling.ObjectPooling_Instance.GetPool(NameManager.NAME_TEXTPOPUPCOIN);
        TextPopupCoin component = textShowCoin.GetComponent<TextPopupCoin>();
        yield return null;
        component.Notification(pos, amount);
        ObjectPooling.ObjectPooling_Instance.ReturnToPool(this.nameitem, this.gameObject);
    }
}
