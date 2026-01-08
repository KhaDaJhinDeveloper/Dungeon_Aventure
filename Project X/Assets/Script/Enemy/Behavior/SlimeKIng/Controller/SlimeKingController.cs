using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKingController : MonoBehaviour
{
    #region State
    private IState currentState;
    #endregion
    #region Components
    private Collider2D coli;
    protected Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator ani;
    private SlimeKingStats stats;
    #endregion
    #region Move
    private Vector2 moveDirection;
    protected float speed;
    #endregion
    #region Spawn
    [SerializeField] private int maxQuantity;
    [SerializeField] private float maxTimeSpawn;
    #endregion
    #region Target
    protected GameObject player;
    #endregion


    public float Speed { get => speed; }
    public Rigidbody2D Rb { get => rb; }
    public GameObject Player { get => player; }
    public int MaxQuantity { get => maxQuantity; }
    public float MaxTimeSpawn { get => maxTimeSpawn; }

    protected virtual void Start()
    {
        LoadComponents();
        SetUp();
    }
    protected virtual void Update()
    {
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
    protected virtual void LoadComponents()
    {
        this.coli = GetComponent<Collider2D>();
        this.rb = GetComponent<Rigidbody2D>();
        this.stats = GetComponent<SlimeKingStats>();
        this.ani = GetComponentInChildren<Animator>();
        this.sprite = GetComponentInChildren<SpriteRenderer>();
        this.player = GameObject.FindWithTag(TagManager.TAG_PLAYER);
    }
    protected virtual void SetUp()
    {
        this.speed = this.stats.Speed;
    }
    public virtual void LockVelocity() => this.rb.velocity = Vector3.zero;
    public virtual void UnLockVelocity() => this.rb.velocity = this.moveDirection* this.speed;
}
