using UnityEngine;

[CreateAssetMenu(menuName = "RPG/ItemEffect/HealByDamage", fileName = "SuckBoolEffect - ")]
public class ItemEffect_HealByDamage : ItemEffectDataSO
{
    [Header("参数")]
    [SerializeField] private float percentHealedOnAttack = .1f;

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);

        EventCenter.DoingDamageEvent += HealOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();

        EventCenter.DoingDamageEvent -= HealOnDoingDamage;
    }

    private void HealOnDoingDamage(float damage)
    {
        float amount = damage * percentHealedOnAttack;
        EventCenter.OnIncreaseHealthEvent(amount);
    }
}
