using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName; 

    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;

    protected float stateTimer; //状态计时器
    protected bool triggerCalled;

    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    protected EntityState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        Debug.Log($"进入了{animBoolName}状态");
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        if(input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }

    public virtual void Exit()
    {
        Debug.Log($"退出了{animBoolName}状态");
        anim.SetBool(animBoolName, false);
    }

    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }

    private bool CanDash()
    {
        if(player.wallDetected)
            return false;

        if(stateMachine.currentState == player.dashState)
            return false;

        return true;
    }
}
