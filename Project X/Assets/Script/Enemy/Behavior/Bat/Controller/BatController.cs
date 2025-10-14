using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BatController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] protected Animator ani;
    [SerializeField] protected Collider2D coli;
    protected BatStats batStats;
    [Header("CheckWall")]
    [SerializeField] protected float groundRadius;
    [SerializeField] protected Transform groundTransform;
    [SerializeField] protected LayerMask nameLayer;
    [Header("PatrolMovement")]
    protected float speedMove;

    IState currentState;
    protected virtual void Start()
    {
        LoadComponent();
    }
    protected virtual void Update()
    {
        if (this.currentState != null)
            this.currentState.Execute();
    }
    protected virtual void FixedUpdate()
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
    protected virtual bool isWall()
    {
        bool isWall = Physics2D.OverlapCircle(this.groundTransform.position, this.groundRadius, this.nameLayer);
        return isWall;
    }
    protected virtual void LoadComponent()
    {
        this.coli = GetComponent<Collider2D>();
        this.ani = GetComponentInChildren<Animator>();
        this.batStats = GetComponent<BatStats>();
        this.speedMove = this.batStats.Speed;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.groundTransform.position, this.groundRadius);
    }
}
