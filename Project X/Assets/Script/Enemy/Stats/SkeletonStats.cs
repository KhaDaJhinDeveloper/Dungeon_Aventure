using NavMeshPlus.Extensions;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonStats : BaseStats
{
    private int life = 1;
    [SerializeField] private int damage;
    public int amountCoin;
    IDamageType damageType;
    private Animator ani;
    private Rigidbody2D rb;
    private NavMeshAgent agent;
    public int Life { get => life; }

    protected override void Start()
    {
        base.Start();
        this.damageType = new PhysicalDamage();
        this.ani = GetComponentInChildren<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        this.agent = GetComponent<NavMeshAgent>();
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
    public override void TakeDamage(int amount, IDamageType damageType, Transform pos)
    {
        base.TakeDamage(amount, damageType, pos);
        if (ThisIsDie())
        {
            this.life -= 1;
            this.currentHealth = Mathf.RoundToInt(this.maxHealth*0.25f);
        }
        else
        {
            StartCoroutine(Effect(this.transform.position, amount));
            UpdateUI();
        }    
    }
    protected override void Die()
    {
        if (life <= 0)
        {
            base.Die();
            StartCoroutine(Death());
        }
    }
    IEnumerator Death()
    {
        rb.simulated = false;
        this.agent.speed = 0; 
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
}
