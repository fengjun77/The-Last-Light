using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;

    private const int FirstComboIndex =  1;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private bool comboAttackQueued;
    private int attackDir;

    private float lastTimeAttacked;

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndex();
        SyncAttackSpeed();

        if(player.moveInput.x != 0)
            attackDir = (int)player.moveInput.x;
        else  
            attackDir = player.facingDir;

        anim.SetInteger("comboIndex", comboIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if(input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        if(triggerCalled)
        {
            if(comboAttackQueued)
            {
                anim.SetBool(animBoolName, false);
                player.EnterAttackStateWithDelay();
            }
            else
                stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttacked = Time.time;
    }

    private void QueueNextAttack()
    {
        if(comboIndex < comboLimit)
            comboAttackQueued = true;
    }
    
    private void ResetComboIndex()
    {
        if (comboIndex > comboLimit || Time.time > lastTimeAttacked + player.comboResetTime)
            comboIndex = FirstComboIndex;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if(attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void ApplyAttackVelocity()
    {
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(player.attackVelocity.x * attackDir, player.attackVelocity.y);
    }
}
