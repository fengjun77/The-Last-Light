using System.Collections;
using UnityEngine;

public class Player : Entity
{
    
    public PlayerInputSet input { get; private set; }

    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; } 
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState attackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }

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

    [Header("攻击相关")]
    public Vector2 attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = .1f;
    public float comboResetTime = 1f;
    private Coroutine queuedAttackCo;

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();
        

        idleState = new Player_IdleState(this, stateMachine, "idle");
        moveState = new Player_MoveState(this, stateMachine, "move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "dash");
        attackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        deadState = new Player_DeadState(this, stateMachine, "dead");
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

    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        EventCenter.OnPlayerDeathEvent();
        stateMachine.ChangeState(deadState);
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
}
