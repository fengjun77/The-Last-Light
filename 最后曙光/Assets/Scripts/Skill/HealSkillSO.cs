using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Skill/Heal", fileName = "Heal LV.")]
public class HealSkillSO : SkillSO
{
    [Header("治疗参数")]
    public float healPercent;

    public GameObject healVfx;

    public override void Cast(Player player)
    {
        Entity_Stats playerStats = player.GetComponent<Entity_Stats>();
        Player_VFX vfx = player.GetComponent<Player_VFX>();
        if(playerStats == null) return;

        float healAmount = playerStats.GetMaxHealth() * healPercent;
        EventCenter.OnIncreaseHealthEvent(healAmount);

        if(vfx != null)
            vfx.CreatePlayerVfx(healVfx, player.transform.position);
    }
}
