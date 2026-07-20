using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Skill/Lightning", fileName = "Lightning LV.")]
public class LightningSkillSO : SkillSO
{
    [Header("闪电参数")]
    public float skillDamageMultiplier; //技能附加伤害百分比
    public float skillRange;
    public GameObject skillFXPrefab;
    public LayerMask enemyLayer;

    public override void Cast(Player player)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, skillRange, enemyLayer);
        foreach(var target in enemies)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            Entity targetEntity = target.GetComponent<Entity>();
            Entity_Stats stats = player.GetComponent<Entity_Stats>();
            Entity_Stats targetStat = target.GetComponent<Entity_Stats>();
            Entity_VFX vfx = player.GetComponent<Entity_VFX>();
            Entity_VFX targetVfx = target.GetComponent<Entity_VFX>();
            
            if(damageable == null) continue;
            if(targetStat == null) continue;

            bool isCrit;
            float damage = stats.GetPhysicalDamage(out isCrit) * (1 + skillDamageMultiplier);
            //自己的护甲穿透
            float armorReduction = stats.GetArmorReduction();
            //对方的护甲减免
            float mitigation = 0;
            if(targetStat != null)
                mitigation = targetStat.GetArmorMitigation(armorReduction);
            float finalDamage = damage * (1 - mitigation);
            
            bool targetGotHit = damageable.TakeDamage(finalDamage, player.transform);
            
            if(targetGotHit)
            {
                vfx.CreateOnHitVFX(target.transform);
                targetVfx.PlayOnStatusVfx(.5f, EffectType.Lightning);
                GameObject prefab = Instantiate(skillFXPrefab, target.transform);
                Destroy(prefab, 2);
                EventCenter.OnDamageNumberEvent(finalDamage, isCrit, targetEntity);
            }
            else
            {
                EventCenter.OnDamageNumberEvent(0, false, targetEntity);
            }
        }
    }
}
