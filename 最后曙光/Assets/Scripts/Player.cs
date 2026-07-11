using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public PlayerInputSet input { get; private set; }

    private StateMachine stateMachine;
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; } 
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState attackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }

    [Header("移动相关")]
    public float moveSpeed;
    public float jumpForce;
    public Vector2 wallJumpForce;
    public float wallJumpDuration; //蹬墙跳多久后可以控制朝向
    public float dashDuration;
    public float dashSpeed;
    [Range(0,1)]
    public float inAirMoveMultiplier = .7f;
    [Range(0,1)]
    public float wallSlideSlowMultiplier = .3f;

    public Vector2 moveInput { get; private set;}

    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    [Header("攻击相关")]
    public Vector2 attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1f;

    [Header("碰撞检测")]
    public GameObject groundCheckPoint;
    [SerializeField] private float groundCheckDistance;
    public GameObject wallCheckPoint;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask groundLayer;
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    private Coroutine queuedAttackCo;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        input = new PlayerInputSet();
        stateMachine = new StateMachine();

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        attackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        stateMachine.Init(idleState);
    }

    void Update()
    {
        stateMachine.currentState.Update();
        HandleCollisionDetection();
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public void EnterAttackStateWithDelay()
    {
        if(queuedAttackCo != null)
            StopCoroutine(queuedAttackCo);
        
        queuedAttackCo = StartCoroutine(AttackStateWithDelay());
    }

    private IEnumerator AttackStateWithDelay()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(attackState);
    }

    public void SetVelocity(float x, float y)
    {
        rb.linearVelocity = new Vector2(x, y);
        HandleFlip(x);
    }

    private void HandleFlip(float xVelocity)
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
    }

    public void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheckPoint.transform.position, Vector2.down, groundCheckDistance, groundLayer);
        wallDetected = Physics2D.Raycast(wallCheckPoint.transform.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckPoint.transform.position, groundCheckPoint.transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(wallCheckPoint.transform.position, wallCheckPoint.transform.position + new Vector3(wallCheckDistance * facingDir, 0));
    }
}
