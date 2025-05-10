using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostStats : BaseStats
{
    [SerializeField] int damage;
    IDamageType damageType;
    private Animator ani;
    private Rigidbody2D rb;
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
        if(collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            BaseStats objPlayer = collision.gameObject.GetComponentInChildren<BaseStats>();
            objPlayer.TakeDamage(this.damage, this.damageType);
        }
    }
    public override void TakeDamage(int amount, IDamageType damageType)
    {
        base.TakeDamage(amount, damageType);
        UpdateUI();
        this.ani.SetTrigger("takehit");
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
        Destroy(gameObject);
    }
    void UpdateUI()
    {
        EventManager.OP_EventManager.TriggerEvent("LoadHp");
        EventManager.OP_EventManager.TriggerEvent("LoadArmor");
        EventManager.OP_EventManager.TriggerEvent("LoadAntimagic");
    }
}
