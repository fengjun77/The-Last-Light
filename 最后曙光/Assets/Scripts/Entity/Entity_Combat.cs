using System.Threading.Tasks;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{  
    private Entity_VFX vfx;
    public float damage = 1;

    [Header("目标检测")]
    public Transform attackCheckPoint;
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private LayerMask targetLayer;

    void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
    }

    public void PerformAttack()
    {
        foreach(var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if(damageable == null) continue;

            damageable.TakeDamage(damage, transform);
            vfx.CreateOnHitVFX(target.transform);
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
