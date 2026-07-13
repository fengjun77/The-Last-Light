using Unity.VisualScripting;
using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        HandelWallSlide();//先检测纵向输出，否则起跳会根据纵向输出改变值

        if(input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }

        if(!player.wallDetected)
        {
            stateMachine.ChangeState(player.fallState);
        }

        if(player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);

            if(player.facingDir != player.moveInput.x)
                player.Flip();
        }

    }

    private void HandelWallSlide()
    {
        if(player.moveInput.y < 0)
        {
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y);
        }
        if(player.moveInput.y == 0)
        {
            player.SetVelocity(player.moveInput.x, rb.linearVelocity.y * player.wallSlideSlowMultiplier);
        }
    }
}
