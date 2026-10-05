using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BatController : MonoBehaviour
{
    [Header("Components")]
    protected Animator ani;
    protected Collider2D coli;
    protected BatStats batStats;
    protected SpriteRenderer sr;
    private Rigidbody2D rb;
    [Header("CheckWall")]
    [SerializeField] protected float rayDistance;
    protected LayerMask nameLayer;
    [Header("DetecPlayer")]
    protected GameObject targetObject;
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected bool isDetecPlayer;
    [SerializeField] protected bool isTrigger;
    [Header("PatrolMovement")]
    [SerializeField] protected float timeChangeDirection;
    [SerializeField] protected LayerMask layerWall;
    protected Vector3 moveDirection;
    protected float speedMove;
    [Header("AttackMovement")]
    [SerializeField] protected bool attackComplated;
    [Header("Flip")]
    [SerializeField] protected bool facingRight;
    [Header("Gizmos")]
    [SerializeField] private bool canSeeGizmos;

    IState currentState;
    public bool IsDetecPlayer { get => isDetecPlayer; set => isDetecPlayer = value; }
    public Animator Ani { get => ani; }
    public Vector3 MoveDirection { get => moveDirection; set => moveDirection = value; }
    public float TimeChangeDirection { get => timeChangeDirection; set => timeChangeDirection = value; }
    public bool AttackComplated { get => attackComplated; set => attackComplated = value; }
    public bool IsTrigger { get => isTrigger; set => isTrigger = value; }
    public GameObject TargetObject { get => targetObject;}
    public LayerMask LayerWall { get => layerWall;}
    public Rigidbody2D Rb { get => rb;  }
    public float SpeedMove { get => speedMove; set => speedMove = value; }
    public float RayDistance { get => rayDistance; set => rayDistance = value; }
    public LayerMask NameLayer { get => nameLayer;}

    protected virtual void Start()
    {
        LoadComponent();
    }
    protected virtual void Update()
    {
        if(this.isTrigger)
            FlipTrigger();
        else FlipPhysics();
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
        this.coli = GetComponent<Collider2D>();
        this.ani = GetComponentInChildren<Animator>();
        this.batStats = GetComponent<BatStats>();
        this.rb = GetComponent<Rigidbody2D>();
        this.targetObject = GameObject.FindWithTag(TagManager.TAG_PLAYER);
        this.sr = GetComponentInChildren<SpriteRenderer>();
        this.speedMove = this.batStats.Speed;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.isTrigger = true;
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
        {
            this.isTrigger = false;
        }
    }
    protected virtual void OnDrawGizmos()
    {
        if (!this.canSeeGizmos) return;
    }
    protected virtual void FlipPhysics()
    {
        if(this.rb.velocity.x > 0 && facingRight)
        {
            this.sr.flipX = false;
            this.facingRight = !facingRight;
        }
        else if (this.rb.velocity.x < 0 && !facingRight)
        {
            this.sr.flipX = true;
            this.facingRight = !facingRight;
        }
    }    
    protected virtual void FlipTrigger()
    {
        if(this.transform.position.x > this.TargetObject.transform.position.x)
            this.sr.flipX = true;
        else if(this.transform.position.x < this.TargetObject.transform.position.x)
            this.sr.flipX = false;
    }    
}
