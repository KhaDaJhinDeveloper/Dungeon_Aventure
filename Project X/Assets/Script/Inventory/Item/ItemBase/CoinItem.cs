using UnityEngine;

public class CoinItem : MonoBehaviour
{
    [SerializeField] private string nameitem;
    [SerializeField] private int amount;

    public string Nameitem { get => nameitem; }
    public int Amount { get => amount; }
    private void Start()
    {
        this.nameitem = KeyClean.CleanKey(this.name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            EventManager.OP_EventManager.TriggerEvent<int>("IncreaseCoinAmount", this.amount);
            ObjectPooling.ObjectPooling_Instance.ReturnToPool(this.nameitem, this.gameObject);
        }
    }
}
