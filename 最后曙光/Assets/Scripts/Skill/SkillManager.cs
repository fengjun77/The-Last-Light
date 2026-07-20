using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    public Player player;
    [Header("技能管理")]
    [SerializeField] private List<SkillSO> availableSkills = new List<SkillSO>();
    [SerializeField] private int currentIndex = 0;
    
    public SkillSO currentSkill => availableSkills.Count > 0 ? availableSkills[currentIndex] : null;
    private Dictionary<SkillSO, float> cooldownDic = new Dictionary<SkillSO, float>();

    void Start()
    {
        EventCenter.OnUpdateSkillsUIEvent(availableSkills);
        EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
    }

    #region 输入回调
    // 上切技能
    public void OnSelectUp()
    {
        PreviousSkill();
    }

    // 下切技能
    public void OnSelectDown()
    {
        NextSkill();
    }
    #endregion

    /// <summary>数字按键直接选中对应下标技能</summary>
    public void SelectByIndex(int index)
    {
        if (availableSkills.Count == 0) return;
        int targetIdx = Mathf.Clamp(index, 0, availableSkills.Count - 1);
        currentIndex = targetIdx;
        EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
    }

    public void AnimationFinished()
    {
        player.CurrentStateAnimationTrigger();
    }

    /// <summary>检测技能是否冷却完成</summary>
    public bool CanCast(SkillSO skill)
    {
        if (skill == null) return false;
        if (!cooldownDic.ContainsKey(skill))
            cooldownDic[skill] = 0;
        return Time.time >= cooldownDic[skill];
    }
    
    /// <summary>释放当前选中技能</summary>
    public void CastSkill()
    {
        if (!CanCast(currentSkill) || currentSkill == null) return;
        
        currentSkill.Cast(player);
        cooldownDic[currentSkill] = Time.time + currentSkill.cooldown;
        EventCenter.OnUpdateSkillCooldownEvent(currentSkill, currentSkill.cooldown);
    }

    #region 核心：等级学习/升级逻辑
    /// <summary>外部调用，判断是否可以学习/升级该卷轴技能</summary>
    public bool CanLearnSkill(SkillSO scrollSkill)
    {
        if (scrollSkill == null) return false;

        // 查找玩家已学的 同skillId 技能
        SkillSO sameIdOwned = GetOwnedSameIdSkill(scrollSkill.skillId);
        // 1. 没有同类型技能，可以直接学
        if (sameIdOwned == null)
            return true;
        // 2. 已有同类型，卷轴等级必须更高才能升级
        return scrollSkill.skillLevel > sameIdOwned.skillLevel;
    }

    /// <summary>根据skillId查找玩家已学的对应技能</summary>
    private SkillSO GetOwnedSameIdSkill(int skillId)
    {
        foreach (var skill in availableSkills)
        {
            if (skill.skillId == skillId)
                return skill;
        }
        return null;
    }

    /// <summary>学习/升级技能，卷轴使用入口</summary>
    public bool LearnSkill(SkillSO scrollSkill)
    {
        // 先校验能否学习，不能直接返回false，不消耗卷轴
        if (!CanLearnSkill(scrollSkill))
            return false;

        SkillSO oldSkill = GetOwnedSameIdSkill(scrollSkill.skillId);
        // 情况1：从未学过该技能，新增进列表
        if (oldSkill == null)
        {
            availableSkills.Add(scrollSkill);
        }
        // 情况2：已有低等级，替换为高等级
        else
        {
            int oldIndex = availableSkills.IndexOf(oldSkill);
            availableSkills[oldIndex] = scrollSkill;
            // 冷却字典移除旧技能，添加新技能
            cooldownDic.Remove(oldSkill);
        }

        // 初始化冷却时间
        if (!cooldownDic.ContainsKey(scrollSkill))
            cooldownDic[scrollSkill] = 0;

        // 修正选中下标防止越界
        currentIndex = Mathf.Clamp(currentIndex, 0, availableSkills.Count - 1);

        // 刷新UI技能栏
        EventCenter.OnUpdateSkillsUIEvent(availableSkills);
        if (availableSkills.Count > 0)
        {
            EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
        }

        return true;
    }
    #endregion

    /// <summary>下一个技能（下方向键）</summary>
    public void NextSkill()
    {
        if (availableSkills.Count == 0) return;
        currentIndex = (currentIndex + 1) % availableSkills.Count;
        EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
    }

    /// <summary>上一个技能（上方向键）</summary>
    public void PreviousSkill()
    {
        if (availableSkills.Count == 0) return;
        currentIndex = (currentIndex - 1 + availableSkills.Count) % availableSkills.Count;
        EventCenter.OnUpdateSkillHighlightEvent(currentSkill);
    }
}
