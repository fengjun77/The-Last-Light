using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Item/SkillScroll", fileName = "SkillScroll - ")]
public class SkillScrollDataSO : ItemDataSO
{
    [Header("技能卷轴专属")]
    public SkillSO bindSkill;    
}
