using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Entity_VFX entityVFX;
    private Entity entity;

    [SerializeField] protected float currentHp;
    [SerializeField] protected float maxHP = 100;
    [SerializeField] protected bool isDead;

    [Header("受击击退")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7,7);
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;

    [Header("重击")]
    [SerializeField] private float heavyDamageThreshold = .3f; //受到超过自身30%生命值的伤害时，视为重击伤害

    protected virtual void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();

        currentHp = maxHP;
        EventCenter.OnHealthChangeEvent(currentHp, maxHP);
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageDealer"></param>
    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if(isDead)
            return;

        Vector2 knockback = CalculateKnockback(damage, damageDealer);
        float duration = CalculateDuration(damage);

        //受到攻击后的击退效果
        entity?.ReciveKnockback(knockback, duration);
        //受击特效
        entityVFX?.PlayeOnDamageVFX();
        ReduceHP(damage);
    }

    /// <summary>
    /// 实际扣血逻辑
    /// </summary>
    /// <param name="damage"></param>
    protected void ReduceHP(float damage)
    {
        currentHp -= damage;
        EventCenter.OnHealthChangeEvent(currentHp, maxHP);

        if(currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;

        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damage) => damage / maxHP >= heavyDamageThreshold;
}
