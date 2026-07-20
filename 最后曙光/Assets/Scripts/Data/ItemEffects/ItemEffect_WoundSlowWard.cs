using UnityEngine;

[CreateAssetMenu(menuName = "RPG/ItemEffect/WoundSlowWard", fileName = "SlowEffect - ")]
public class ItemEffect_WoundSlowWard : ItemEffectDataSO
{
    [Header("参数")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float range;
    [SerializeField] private float duration;

    [Header("触发概率")]
    [SerializeField] private float triggerPercent = 5;

    [Header("效果")]
    public GameObject effectVfx;
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();

        if(Random.Range(0,100) > triggerPercent) return;

        player.vfx.CreatePlayerVfx(effectVfx, player.transform.position);
    
        DamageEnemiesWithSlow();
    }

    private void DamageEnemiesWithSlow()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, range, enemyLayer);
    
        foreach(var target in enemies)
        {
            Entity targetEntity = target.GetComponent<Entity>();

            Entity_VFX targetVfx = target.GetComponent<Entity_VFX>();

            if(targetEntity == null || targetVfx == null) continue;

            targetVfx.PlayOnStatusVfx(duration, EffectType.Chill);
            targetEntity.SlowDownEntityBy(duration, 1);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);

        EventCenter.TakingDamageEvent += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        EventCenter.TakingDamageEvent -= ExecuteEffect;
    
        player = null;
    }
}
