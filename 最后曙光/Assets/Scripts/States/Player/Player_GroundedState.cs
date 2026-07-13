using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if(rb.linearVelocity.y <= -0.1f && !player.groundDetected)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if(input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if(input.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.attackState);
        }

        if(input.Player.Counter.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
    }
}
