using UnityEngine;

public class Player_WallJumpState : EntityState
{
    private float controlDelayTimer;
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controlDelayTimer = player.wallJumpDuration;

        player.SetVelocity(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        if(input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpAttackState);

        if (controlDelayTimer > 0f)
        {
            controlDelayTimer -= Time.deltaTime;
        }
        else
        {
            if (Mathf.Abs(player.moveInput.x) > 0.1f)
            {
                player.SetVelocity(player.moveInput.x * player.moveSpeed * player.inAirMoveMultiplier, rb.linearVelocity.y);
            }
        }

        if(rb.linearVelocity.y <= -0.1f && stateMachine.currentState != player.jumpAttackState)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if(player.wallDetected)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
