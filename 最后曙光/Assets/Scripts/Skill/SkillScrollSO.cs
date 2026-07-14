using UnityEngine;

[CreateAssetMenu(menuName = "Loot/SkillScroll", fileName = "Scroll_")]
public class SkillScrollSO : CollectibleSO
{
    [Header("卷轴对应技能")]
    public SkillSO targetSkill;

    public override void Collect(Player player)
    {
        if (targetSkill != null)
        {
            player.skillManager.LearnSkill(targetSkill);
        }
        //EventCenter.OnShowTipEvent(this);
    }
}
