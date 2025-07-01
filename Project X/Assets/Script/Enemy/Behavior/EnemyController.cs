using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public BaseStats baseStats;
    protected IState currentState;
    public Vector2 moveDirection;
    public LayerMask nameLayer;
    public float rayDistance;
    public float enemySize;
    public bool canSeeTarget = false;
    public bool canAttack;
    public float attackCoolDown;
    [SerializeField] protected float detectionRange;
    [SerializeField] protected float attackRange;
    public Transform playerTransform;
    public Rigidbody2D rb;
    [SerializeField] protected EnemyType[] enemyType;
    protected Animator ani;
    protected SpriteRenderer sr;
    public bool showDebugRay;
    public Animator Ani { get => ani; set => ani = value; }
    public SpriteRenderer Sr { get => sr; set => sr = value; }
    public float DetectionRange { get => detectionRange; set => detectionRange = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }

    protected virtual void OnEnable()
    {
        
    }
    protected virtual void Start()
    {
        LoadComponent();
    }
    protected virtual void LoadComponent()
    {
        ani = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        baseStats = GetComponent<BaseStats>();
        playerTransform = GameObject.FindWithTag(TagManager.TAG_PLAYER).transform;
    }    
    protected virtual void Update()
    {
        if (currentState != null)
            currentState.Execute();
    }

    protected virtual void FixedUpdate()
    {
        if (currentState != null)
            currentState.Execute();
    }
    protected virtual void ChangeState(IState state)
    {
        if(currentState != null && currentState.GetType() == state.GetType())
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
    public virtual bool IsPlayerRangeChase(float range)
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < range)
            return true;
        else
            return false;
    }
    public virtual bool IsPlayerRangeAttack(float range)
    {
        if (Vector2.Distance(transform.position, playerTransform.position) < range)
            return true;
        else
            return false;
    }
    protected virtual Vector2 ChooseRanDomDirection(Vector2 Vectorrandom)
    {
        Vectorrandom = Random.insideUnitCircle.normalized;
        return Vectorrandom;
    }     
    public virtual void Flip()
    {
       
    }
    public void OnDrawGizmos()
    {
        if(!showDebugRay) return;
        Gizmos.color = canSeeTarget ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemySize);
        if (playerTransform != null)
        {
            Gizmos.color = canSeeTarget ? Color.red : Color.gray;
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }
        if (Application.isPlaying && moveDirection != Vector2.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, moveDirection * 1.5f);
        }
    }
}
    