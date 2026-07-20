using UnityEngine;

public abstract class SkillSO : ScriptableObject
{
    [Header("技能基础信息")]
    public string skillName;
    public Sprite icon;
    public float cooldown;

    [Header("等级系统")]
    [Tooltip("同一套技能共享SkillId，区分不同等级")]
    public int skillId;
    [Tooltip("当前技能等级")]
    public int skillLevel;

    [TextArea]
    public string skillDescription;

    public abstract void Cast(Player player);
}
