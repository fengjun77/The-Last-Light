using UnityEngine;

public class UI_DamageNumber : MonoBehaviour
{
    [Header("伤害数字预制体")]
    public DamageNumber damageNumberPrefab;
    [Header("对象池预生成数量")]
    public int poolInitCount = 20;

    private Transform poolRoot;

    private void Start()
    {
        poolRoot = transform;

        PoolManager.Instance.CreatePool<DamageNumber>(damageNumberPrefab, poolRoot, poolInitCount);
    }

    private void OnEnable()
    {
        // 全局监听受伤事件
        EventCenter.OnHit += SpawnDamageText;
        EventCenter.OnFlip += HandleFlip;
    }

    private void OnDisable()
    {
        EventCenter.OnHit -= SpawnDamageText;
        EventCenter.OnFlip -= HandleFlip;
    }

    /// <summary>根据受伤实体，在头顶生成伤害数字</summary>
    public void SpawnDamageText(float damage, bool isCrit, Entity hurtEntity)
    {
        if (hurtEntity == null || damageNumberPrefab == null) return;

        if (hurtEntity.GetComponent<Enemy>() == null) return;

        // 从对象池取出
        DamageNumber num = PoolManager.Instance.GetItem<DamageNumber>(damageNumberPrefab);
        if (num == null) return;

        // 绑定预制体用于归还
        num.damagePrefab = damageNumberPrefab;

        Vector3 worldPos = hurtEntity.GetComponent<Enemy>().damageNumRoot.position + new Vector3(0, 1f, 0);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // 放到受伤实体头顶
        num.transform.position = screenPos;
        num.SetValue(damage, isCrit);
    }

    private void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
