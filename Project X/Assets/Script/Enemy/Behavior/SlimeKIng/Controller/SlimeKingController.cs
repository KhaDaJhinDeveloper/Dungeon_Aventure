using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SlimeKingController : MonoBehaviour
{
    #region State
    protected IState currentState;
    #endregion
    #region Components
    protected Collider2D coli;
    protected Rigidbody2D rb;
    protected SpriteRenderer sprite;
    protected Animator ani;
    protected SlimeKingStats stats;
    #endregion
    #region Target
    protected GameObject player;
    #endregion
    #region MoveState
    protected Vector2 moveDirection;
    protected float speed;
    #endregion
    [Header("SpawnState")]
    [SerializeField] protected int maxQuantity;
    [SerializeField] protected float maxTimeSpawn;
    protected bool allowSpawn;
    [Header("CoolDown")]
    [SerializeField] protected float CoolDownAttack;
    [SerializeField] protected float CoolDownSpawn;
    protected float currentCoolDownAttack;
    protected float currentCoolDownSpawn;
    [Header("AttackState")]
    #region IndexDefault
    [SerializeField] protected float maxJumpUpDuration;
    [SerializeField] protected float maxJumpHeight;
    #endregion
    protected float jumpUpDuration;
    [SerializeField] protected float hoverDuration;
    [SerializeField] protected float fallDownDuration;
    [SerializeField] protected float jumpHeight;
    [SerializeField] protected int numbersOfJump;
    [SerializeField] protected float delayBetweenJumps;
    protected bool attackCompleted;
    protected bool allowAttack;
    [Header("Range")]
    [SerializeField] protected float rangeAttack;
    [SerializeField] protected float rangeChase;

    public float Speed { get => speed; }
    public Rigidbody2D Rb { get => rb; }
    public GameObject Player { get => player; }
    public int MaxQuantity { get => maxQuantity; }
    public float MaxTimeSpawn { get => maxTimeSpawn; }
    public float JumpUpDuration { get => jumpUpDuration; set => jumpUpDuration = Mathf.Max(0, value); }
    public float HoverDuration { get => hoverDuration; }
    public float FallDownDuration { get => fallDownDuration; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = Mathf.Max(0, value); }
    public int NumbersOfJump { get => numbersOfJump; }
    public float DelayBetweenJumps { get => delayBetweenJumps; }
    public Animator Ani { get => ani; }

    protected virtual void Start()
    {
        LoadComponents();
        SetUp();
    }
    protected virtual void Update()
    {
        if (this.currentState != null)
            this.currentState.Execute();
        if (this.stats.ThisIsDie()) this.currentState = null;
    }
    protected virtual void ChangeState(IState state)
    {
        if (currentState != null && currentState.GetType() == state.GetType())
            return;
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = state;
        if (currentState != null)
        {
            currentState.Enter();
        }
    }
    protected virtual void LoadComponents()
    {
        this.coli = GetComponent<Collider2D>();
        this.rb = GetComponent<Rigidbody2D>();
        this.stats = GetComponent<SlimeKingStats>();
        this.ani = GetComponentInChildren<Animator>();
        this.sprite = GetComponentInChildren<SpriteRenderer>();
        this.player = GameObject.FindWithTag(TagManager.TAG_PLAYER);
    }
    public virtual void SetUp()
    {
        this.speed = this.stats.Speed;
        this.jumpUpDuration = this.maxJumpUpDuration;
        this.JumpHeight = this.maxJumpHeight;
    }
    protected virtual bool IsRangeChase()
    {
        float distance = Vector3.Distance(this.transform.position, this.player.transform.position);
        return distance < this.rangeChase;
    }
    protected virtual bool IsRangeAttack()
    {
        float distance = Vector3.Distance(this.transform.position, this.player.transform.position);
        return distance < this.rangeAttack;
    }
    protected virtual void FlipTransform()
    {
        if (this.transform.position.x > this.player.transform.position.x)
            this.sprite.flipX = true;
        else if (this.transform.position.x < this.player.transform.position.x)
            this.sprite.flipX = false;
    }
    public virtual void LockVelocity() => this.rb.velocity = Vector3.zero;
    public virtual void UnLockVelocity() => this.rb.velocity = this.moveDirection* this.speed;
    public virtual bool AllowAttack() => this.attackCompleted = false;
    public virtual bool BlockAttack() => this.attackCompleted = true;  
    public virtual void EnabledCollider2D() => this.coli.enabled = true;
    public virtual void DisabledCollider2D() => this.coli.enabled = false;
}
