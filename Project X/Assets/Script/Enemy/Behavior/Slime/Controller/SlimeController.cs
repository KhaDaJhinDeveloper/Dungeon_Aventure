using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlimeController : MonoBehaviour
{
    #region Components
    protected Rigidbody2D rb;
    protected Animator ani;
    protected Collider2D colli;
    protected BaseStats slimeStats;
    protected SpriteRenderer sr;
    #endregion

    #region Movement
    protected Vector2 moveDirection;
    protected int speedMove;
    protected IState currentState;
    [SerializeField] protected float maxTime;
    protected float currentTime = 0;
    #endregion

    #region CheckWall  
    protected Transform wallCheck;
    [SerializeField] protected float rayDistance;
    [SerializeField] protected LayerMask wallLayer;
    #endregion

    #region Flip
    protected bool facingRight;
    #endregion
    public Vector2 MoveDirection { get => moveDirection; set => this.moveDirection = value; }
    public float RayDistance { get => rayDistance; }
    public Transform WallCheck { get => wallCheck; }
    public LayerMask WallLayer { get => wallLayer; }
    public Animator Ani { get => ani; }
    public BaseStats SlimeStats { get => slimeStats; }

    protected virtual void Start()
    {
        LoadComponent();
    }
    protected virtual void Update()
    {   
        Flip();
        if (this.currentState != null)
            this.currentState.Execute();
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
    protected virtual void LoadComponent()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.colli = GetComponent<Collider2D>();
        this.slimeStats = GetComponent<BaseStats>();
        this.ani = GetComponentInChildren<Animator>();
        this.sr = GetComponentInChildren<SpriteRenderer>();
        this.wallCheck = GetComponent<Transform>();
        this.speedMove = this.slimeStats.Speed;
    }
    public void LockVelocity() => this.rb.velocity = Vector2.zero;
    public void UnLockVelocity() => this.rb.velocity = this.moveDirection * speedMove;
    protected void Flip()
    {
        if(this.rb.velocity.x > 0 && this.facingRight)
        {
            this.sr.flipX = false;
            this.facingRight = !facingRight;
        }
        else if(this.rb.velocity.x < 0 && !this.facingRight)
        {
            this.sr.flipX = true;
            this.facingRight = !facingRight;
        }
    }
}
