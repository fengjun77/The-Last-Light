using System.Threading.Tasks;
using UnityEditor.Build;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{  
    private Entity_VFX vfx;
    private Entity_Stats stats;

    [Header("目标检测")]
    public Transform attackCheckPoint;
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private LayerMask targetLayer;

    void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        foreach(var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            Entity targetEntity = target.GetComponent<Entity>();
            Entity_Stats targetStat = target.GetComponent<Entity_Stats>();
            
            if(damageable == null) continue;

            bool isCrit;
            float damage = stats.GetPhysicalDamage(out isCrit);
            //自己的护甲穿透
            float armorReduction = stats.GetArmorReduction();
            //对方的护甲减免
            float mitigation = 0;
            if(targetStat != null)
                mitigation = targetStat.GetArmorMitigation(armorReduction);
            float finalDamage = damage * (1 - mitigation);
            
            bool targetGotHit = damageable.TakeDamage(finalDamage, transform);
            
            if(targetGotHit)
            {
                vfx.CreateOnHitVFX(target.transform);
                EventCenter.OnHitEvent(finalDamage, isCrit, targetEntity);
            }
            else
            {
                EventCenter.OnHitEvent(0, false, targetEntity);
            }
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(attackCheckPoint.position, attackCheckRadius, targetLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheckPoint.position, attackCheckRadius);
    }
}
