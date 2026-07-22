using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Entity_Stats stats { get; private set; }
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;

    [Header("掉落经验")]
    public int dropExp;

    [Header("移动参数")]
    public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0,2)]
    public float moveAnimSpeedMultiplier;

    [Header("眩晕参数")]
    public float stunnedDuration;
    public Vector2 stunnedVelocity = new Vector2(7,7);
    protected bool canBeStunned;

    [Header("追击参数")]
    public float battleMoveSpeed = 3f;
    public float attackDistance;
    public float battleTimeDuration = 5;
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("玩家检测")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float playerCheckDistance;
    public GameObject playerCheckPoint;

    public Transform player { get; private set; }

    public Transform damageNumRoot;

    protected override void Awake()
    {
        base.Awake();
        stats = GetComponent<Entity_Stats>();
    }

    void OnEnable()
    {
        EventCenter.PlayerDeathEvent += HandlePlayerDeath;
    }

    void OnDisable()
    {
        EventCenter.PlayerDeathEvent -= HandlePlayerDeath; 
    }

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalBattleSpeed = battleMoveSpeed;
        float originalAnimSpeed = anim.speed;

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed = moveSpeed * speedMultiplier;
        battleMoveSpeed = battleMoveSpeed * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        battleMoveSpeed = originalBattleSpeed;
        anim.speed = originalAnimSpeed;
    }

    public void EnableCounterWindow(bool enable) => canBeStunned = enable;

    public override void EntityDeath()
    {
        base.EntityDeath();

        stateMachine.ChangeState(deadState);
        EventCenter.OnAddExpEvent(dropExp);

        Destroy(gameObject, 2f);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    public void TryEnterBattleState(Transform player)
    {
        if(stateMachine.currentState == battleState || stateMachine.currentState == attackState)
            return;

        this.player = player;
        stateMachine.ChangeState(battleState);        
    }

    public Transform GetPlayerReference()
    {
        if(player == null)
            player = PlayerDetected().transform;

        return player;
    }

    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheckPoint.transform.position, Vector2.right * facingDir, playerCheckDistance, playerLayer | groundLayer);

        if(hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheckPoint.transform.position, playerCheckPoint.transform.position + new Vector3(facingDir * playerCheckDistance, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheckPoint.transform.position, playerCheckPoint.transform.position + new Vector3(facingDir * attackDistance, 0));
    }
}
