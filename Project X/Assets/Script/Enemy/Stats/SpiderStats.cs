using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderStats : BaseStats
{
    [SerializeField] private int damage;
    public int amountCoin;
    IDamageType damageType;
    private Animator ani;
    private Rigidbody2D rb;
    private float timePoision = 5f;
    protected override void Start()
    {
        base.Start();
        this.damageType = new MagicalDamage();
        this.ani = GetComponentInChildren<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        UpdateUI();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            BaseStats objPlayer = collision.gameObject.GetComponentInChildren<BaseStats>();
            objPlayer.TakeDamage(this.damage, this.damageType, this.transform);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            BaseStats objPlayer = collision.gameObject.GetComponentInChildren<BaseStats>();
            StartCoroutine(Poison(objPlayer));
        }
    }
    public override void TakeDamage(int amount, IDamageType damageType, Transform pos)
    {
        base.TakeDamage(amount, damageType, pos);
        StartCoroutine(Effect(this.transform.position, amount));
        UpdateUI();
    }
    protected override void Die()
    {
        base.Die();
        StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        rb.simulated = false;
        this.ani.SetTrigger("death");
        yield return new WaitForSeconds(1f);
        ItemDropSpawn.itemDropSpawn_Instance.DropItem(this.transform.position, this.amountCoin);
        Destroy(gameObject);
    }
    public override void UpdateUI()
    {
        EventManager.OP_EventManager.TriggerEvent("LoadHp");
        EventManager.OP_EventManager.TriggerEvent("LoadArmor");
        EventManager.OP_EventManager.TriggerEvent("LoadAntimagic");
    }
    IEnumerator Effect(Vector3 pos, int amount)
    {
        this.ani.SetTrigger("takehit");
        GameObject textShowDamage = ObjectPooling.ObjectPooling_Instance.GetPool(NameManager.NAME_TEXTPOPUPDAMAGE);
        TextShowDamage component = textShowDamage.GetComponent<TextShowDamage>();
        yield return null;
        component.Notification(pos, amount);
    }
    IEnumerator Poison(BaseStats target)
    {
        float currentTime = 0;
        while(currentTime < this.timePoision)
        {
            target.TakeDamage(this.damage, this.damageType, this.transform);
            yield return new WaitForSeconds(1);
            currentTime += 1;
        }
        target = null;
    }
}
