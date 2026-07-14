using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventCenter
{
    public static event Action OnPlayerDeath;
    public static void OnPlayerDeathEvent()
    {
        OnPlayerDeath?.Invoke();
    }

    public static event Action<float, bool, Entity> OnHit;
    public static void OnHitEvent(float damage, bool isCrit, Entity target)
    {
        OnHit?.Invoke(damage, isCrit, target);
    }

    public static event Action<float, float, Entity> OnHealthChange;
    public static void OnHealthChangeEvent(float currentHp, float maxHP, Entity target)
    {
        OnHealthChange?.Invoke(currentHp, maxHP, target);
    }

    public static event Action OnFlip;
    public static void OnFlipEvent()
    {
        OnFlip?.Invoke();
    }

    public static event Action<List<SkillSO>> UpdateSkillsUIEvent;
    public static event Action<SkillSO> UpdateSkillHighlightEvent;
    public static event Action<SkillSO, float> UpdateSkillCooldownEvent;

    public static void OnUpdateSkillsUIEvent(List<SkillSO> list) => UpdateSkillsUIEvent?.Invoke(list);
    public static void OnUpdateSkillHighlightEvent(SkillSO skill) => UpdateSkillHighlightEvent?.Invoke(skill);
    public static void OnUpdateSkillCooldownEvent(SkillSO skill, float cd) => UpdateSkillCooldownEvent?.Invoke(skill, cd);
}
