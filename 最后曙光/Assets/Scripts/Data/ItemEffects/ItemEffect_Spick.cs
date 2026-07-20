using UnityEngine;

[CreateAssetMenu(menuName = "RPG/ItemEffect/Spick", fileName = "SpickEffect - ")]
public class ItemEffect_Spick : ItemEffectDataSO
{
    public LayerMask enemyLayer;
    [Header("触发概率")]
    [SerializeField] private float triggerPercent;

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();

        if(Random.Range(0,100) > triggerPercent) return;

        DamageEnemies();
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);

        EventCenter.TakingDamageEvent += ExecuteEffect;
    }

    private void DamageEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, enemyLayer);
    
        foreach(var target in enemies)
        {
            Enemy_Health health = target.GetComponent<Enemy_Health>();

            if(health == null) continue;

            health.TakeDamage(player.stats.GetMaxHealth() * .1f, player.transform);
        }
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        EventCenter.TakingDamageEvent -= ExecuteEffect;
    
        player = null;
    }
}
