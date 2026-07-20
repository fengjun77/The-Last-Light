using UnityEngine;

[CreateAssetMenu(menuName = "RPG/ItemEffect/Heal", fileName = "HealEffect - ")]
public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healPercent = .1f;

    public override void ExecuteEffect()
    {
        Player player = FindFirstObjectByType<Player>();

        float healAmount = player.stats.GetMaxHealth() * healPercent;

        EventCenter.OnIncreaseHealthEvent(healAmount);
    }
}
