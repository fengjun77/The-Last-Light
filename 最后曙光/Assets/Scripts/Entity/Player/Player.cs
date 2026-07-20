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
    public Player_CastSkillState castSkillState { get; private set; }
    public Player_DeadState deadState { get; private set; }

    public Player_VFX vfx { get; private set; }

    public SkillManager skill;
    public UI ui;

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
        
        vfx = GetComponent<Player_VFX>();

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
        castSkillState = new Player_CastSkillState(this, stateMachine, "castSkill");
        deadState = new Player_DeadState(this, stateMachine, "dead");
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;

        input.Player.Interact.performed += ctx => TryInteract();

        input.UI.SelectSkillUp.performed += ctx => skill.OnSelectUp();
        input.UI.SelectSkillDown.performed += ctx => skill.OnSelectDown();
        input.UI.ToggleInventoryUI.performed += ctx => ui.ToggleInventoryUI();

        input.UI.SelectSkill1.performed += _ => skill.SelectByIndex(0);
        input.UI.SelectSkill2.performed += _ => skill.SelectByIndex(1);
        input.UI.SelectSkill3.performed += _ => skill.SelectByIndex(2);
        input.UI.SelectSkill4.performed += _ => skill.SelectByIndex(3);
        input.UI.SelectSkill5.performed += _ => skill.SelectByIndex(4);
        input.UI.SelectSkill6.performed += _ => skill.SelectByIndex(5);
        input.UI.SelectSkill7.performed += _ => skill.SelectByIndex(6);
        input.UI.SelectSkill8.performed += _ => skill.SelectByIndex(7);
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

    private void TryInteract()
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        Collider2D[] objectsAround = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach(var target in objectsAround)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            if(interactable == null) continue;

            float distance = Vector2.Distance(transform.position, target.transform.position);

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closest = target.transform;
            }
        }

        if(closest == null) return;

        closest.GetComponent<IInteractable>().Interact();
    }
}
