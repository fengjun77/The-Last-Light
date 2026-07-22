using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Skill/Frost Field", fileName = "FrostField LV.")]
public class SlowDownSkillSO : SkillSO
{
    [Header("减速参数")]
    public float slowDuration;
    public float slowMultiplier;
    public float skillRange;
    public LayerMask enemyLayer;

    public override void Cast(Player player)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, skillRange, enemyLayer);
    
        foreach(var target in enemies)
        {
            Entity targetEntity = target.GetComponent<Entity>();

            Entity_VFX targetVfx = target.GetComponent<Entity_VFX>();
            Entity_SFX sfx = player.GetComponent<Entity_SFX>();

            if(targetEntity == null || targetVfx == null) continue;

            targetVfx.PlayOnStatusVfx(slowDuration, EffectType.Chill);
            targetEntity.SlowDownEntityBy(slowDuration, slowMultiplier);
            sfx.PlaySlowEffect();
        }
    }
}
