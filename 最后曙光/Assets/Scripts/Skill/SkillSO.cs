using UnityEngine;

public abstract class SkillSO : CollectibleSO
{
    [Header("技能通用")]
    public float cooldown;
    
    public override void Collect(Player player)
    {
        player.skillManager.LearnSkill(this);
        //EventCenter.OnShowTipEvent(this);
    }
    
    public abstract void Cast(Player player);
}
