using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachine;

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    [Header("碰撞检测")]
    public GameObject groundCheckPoint;
    [SerializeField] private float groundCheckDistance;
    public GameObject wallCheckPoint_Hand;
    public GameObject wallCheckPoint_Foot;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] protected LayerMask groundLayer;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    public bool isKnocked;
    private Coroutine knockbackCo;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        stateMachine.currentState.Update();
        HandleCollisionDetection();
    }

    public void CurrentStateAnimationTrigger()
    {
        stateMachine.currentState.AnimationTrigger();
    }

    public virtual void EntityDeath()
    {
        
    }

    public void SetVelocity(float x, float y)
    {
        if(isKnocked)
            return;

        rb.linearVelocity = new Vector2(x, y);
        HandleFlip(x);
    }

    public void ReciveKnockback(Vector2 knockback, float duration)
    {
        if(knockbackCo != null)
            StopCoroutine(knockbackCo);

        knockbackCo = StartCoroutine(KnockbackCo(knockback, duration));
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;

        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }

    public void HandleFlip(float xVelocity)
    {
        if(xVelocity > 0 && facingRight == false) //原本面向左
        {
            Flip();
        }
        else if(xVelocity < 0 && facingRight == true) //原本面向右
        {
            Flip();
        }
    }

    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir *= -1;

        //防止血条翻转
        EventCenter.OnFlipEvent();
    }

    public void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheckPoint.transform.position, Vector2.down, groundCheckDistance, groundLayer);
        
        if(wallCheckPoint_Foot != null)
            wallDetected = Physics2D.Raycast(wallCheckPoint_Hand.transform.position, Vector2.right * facingDir, wallCheckDistance, groundLayer)
                        && Physics2D.Raycast(wallCheckPoint_Foot.transform.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
        else
            wallDetected = Physics2D.Raycast(wallCheckPoint_Hand.transform.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckPoint.transform.position, groundCheckPoint.transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(wallCheckPoint_Hand.transform.position, wallCheckPoint_Hand.transform.position + new Vector3(wallCheckDistance * facingDir, 0));
        if(wallCheckPoint_Foot != null)
            Gizmos.DrawLine(wallCheckPoint_Foot.transform.position, wallCheckPoint_Foot.transform.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
