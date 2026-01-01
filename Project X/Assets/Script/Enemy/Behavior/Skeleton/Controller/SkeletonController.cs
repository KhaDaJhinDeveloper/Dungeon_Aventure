using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour
{
    #region State;
    protected IState currentState;
    #endregion
    #region Component
    protected Rigidbody2D rb;
    protected Collider2D colli;
    protected SkeletonStats stats;
    protected Animator ani;
    protected NavMeshAgent agent;
    protected SpriteRenderer sprite;
    #endregion
    #region Flip
    protected bool facingRight;
    #endregion
    #region Movement
    protected Vector2 moveDirection;
    protected int speedMove;
    #region Trigger
    protected bool isTrigger;
    protected Transform target;
    [SerializeField] protected LayerMask nameLayerTarget;
    [SerializeField] protected LayerMask wallLayer;
    #endregion 
    protected bool allowKnockBack;

    public bool IsTrigger { get => isTrigger; }
    public Rigidbody2D Rb { get => rb; }
    public Collider2D Colli { get => colli; }
    public bool AllowKnockBack { get => allowKnockBack; set => allowKnockBack = value; }
    public Transform Target { get => target; }
    public SkeletonStats Stats { get => stats; }
    public LayerMask WallLayer { get => wallLayer; }
    public NavMeshAgent Agent { get => agent; }
    public Animator Ani { get => ani; }
    #endregion


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
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & nameLayerTarget.value) > 0)
        {
            KnockBack(collision.gameObject.transform);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagManager.TAG_PLAYER))
        {
            this.isTrigger = true;  
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagManager.TAG_PLAYER))
        {           
            this.isTrigger = false;
        }
    }
    protected void KnockBack(Transform target)
    {
        if(!this.allowKnockBack || target == null) return;
        Vector3 distance = (this.transform.position - target.position).normalized;
        Vector3 newPos = new Vector3(
                                     transform.position.x + distance.x * 0.2f ,
                                     transform.position.y + distance.y * 0.2f , 
                                     0f ); 
        transform.position = newPos;
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
    public virtual void LockVelocity() => this.rb.velocity = Vector2.zero;
    public virtual void UnlockVelocity() => this.rb.velocity = this.moveDirection * this.speedMove;
    protected virtual void LoadComponents()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.colli = GetComponent<Collider2D>();
        this.stats = GetComponent<SkeletonStats>();
        this.ani = GetComponentInChildren<Animator>();
        this.sprite = GetComponentInChildren<SpriteRenderer>();
        this.agent = GetComponent<NavMeshAgent>();
        this.target = GameObject.FindWithTag(TagManager.TAG_PLAYER).transform; 
    }
    protected virtual void SetUp()
    {
        this.agent.updateRotation = false;
        this.agent.updateUpAxis = false;
    } 
    protected virtual void FlipPhysics()
    {
        if(this.rb.velocity.x > 0 && this.facingRight)
        {
            this.sprite.flipX = !this.sprite.flipX;
            this.facingRight = !this.facingRight;
        }    
        else if(this.rb.velocity.x < 0 && !this.facingRight)
        {
            this.sprite.flipX = !this.sprite.flipX;
            this.facingRight = !this.facingRight;
        }
    }
    protected virtual void FlipTransform()
    {
        if(transform.position.x < this.target.position.x)
            this.sprite.flipX = false;
        else this.sprite.flipX = true;
    }
}
