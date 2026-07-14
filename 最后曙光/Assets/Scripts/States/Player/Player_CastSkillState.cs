using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player_CastSkillState : PlayerState
{
    public Player_CastSkillState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
        {
            if(Math.Abs(player.moveInput.x) > 0.1f)
                stateMachine.ChangeState(player.moveState);
            else
                stateMachine.ChangeState(player.idleState);
        }
    }
}
