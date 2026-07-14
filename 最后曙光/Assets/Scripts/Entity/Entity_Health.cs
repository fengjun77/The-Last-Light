using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Entity_VFX entityVFX;
    private Entity entity;
    private Entity_Stats entityStats;

    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead;

    [Header("Health regen")]
    [SerializeField] private float regenInterval = 2;
    [SerializeField] private bool canRegenerateHealth = true;

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
        entityStats = GetComponent<Entity_Stats>();

        currentHealth = entityStats.GetMaxHealth();
        EventCenter.OnHealthChangeEvent(currentHealth, entityStats.GetMaxHealth(), entity);
    
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="damageDealer"></param>
    public virtual bool TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            Debug.Log("成功闪避");
            return false;
        }

        TakeKnockback(damage, damageDealer);
        //受击特效
        entityVFX?.PlayeOnDamageVFX();
        ReduceHealth(damage);

        return true;
    }

    /// <summary>
    /// 攻击失败
    /// </summary>
    /// <returns></returns>
    private bool AttackEvaded()
    {
        return Random.Range(0,100) < entityStats.GetEvasion();
    }

    private void RegenerateHealth()
    {
        if(!canRegenerateHealth) return;

        float regenAmount = entityStats.resource.healthRegen.GetValue();

        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if(isDead)
            return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = entityStats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth, maxHealth);

        EventCenter.OnHealthChangeEvent(currentHealth, entityStats.GetMaxHealth(), entity);
    }

    /// <summary>
    /// 实际扣血逻辑
    /// </summary>
    /// <param name="damage"></param>
    protected void ReduceHealth(float damage)
    {
        currentHealth -= damage;
        EventCenter.OnHealthChangeEvent(currentHealth, entityStats.GetMaxHealth(), entity);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    private void TakeKnockback(float damage, Transform damageDealer)
    {
        Vector2 knockback = CalculateKnockback(damage, damageDealer);
        float duration = CalculateDuration(damage);

        //受到攻击后的击退效果
        entity?.ReciveKnockback(knockback, duration);
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;

        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damage) => damage / entityStats.GetMaxHealth() >= heavyDamageThreshold;
}
