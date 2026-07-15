using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventCenter
{
    public static event Action PlayerDeathEvent;
    public static void OnPlayerDeathEvent()
    {
        PlayerDeathEvent?.Invoke();
    }

    public static event Action<float, bool, Entity> HitEvent;
    public static void OnHitEvent(float damage, bool isCrit, Entity target)
    {
        HitEvent?.Invoke(damage, isCrit, target);
    }

    public static event Action<float, float, Entity> HealthChangeEvent;
    public static void OnHealthChangeEvent(float currentHp, float maxHP, Entity target)
    {
        HealthChangeEvent?.Invoke(currentHp, maxHP, target);
    }

    public static event Action FlipEvent;
    public static void OnFlipEvent()
    {
        FlipEvent?.Invoke();
    }

    public static event Action<List<SkillSO>> UpdateSkillsUIEvent;
    public static event Action<SkillSO> UpdateSkillHighlightEvent;
    public static event Action<SkillSO, float> UpdateSkillCooldownEvent;

    //更新技能栏
    public static void OnUpdateSkillsUIEvent(List<SkillSO> list) => UpdateSkillsUIEvent?.Invoke(list);
    //更新技能选中高光
    public static void OnUpdateSkillHighlightEvent(SkillSO skill) => UpdateSkillHighlightEvent?.Invoke(skill);
    //更新技能冷却
    public static void OnUpdateSkillCooldownEvent(SkillSO skill, float cd) => UpdateSkillCooldownEvent?.Invoke(skill, cd);

    
    public static event Action InventoryChangeEvent;

    public static void OnInventoryChangeEvent()
    {
        InventoryChangeEvent?.Invoke();
    }
}
