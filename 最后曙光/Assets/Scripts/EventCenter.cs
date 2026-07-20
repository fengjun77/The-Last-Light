using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public static class EventCenter
{
    public static event Action PlayerDeathEvent;
    public static void OnPlayerDeathEvent()
    {
        PlayerDeathEvent?.Invoke();
    }

    public static event Action<float, bool, Entity> DamageNumberEvent;
    public static void OnDamageNumberEvent(float damage, bool isCrit, Entity target)
    {
        DamageNumberEvent?.Invoke(damage, isCrit, target);
    }

    //受到伤害后触发的事件
    public static event Action TakingDamageEvent;
    public static void OnTakingDamageEvent()
    {
        TakingDamageEvent?.Invoke();
    }

    //造成伤害后触发的事件
    public static event Action<float> DoingDamageEvent;
    public static void OnDoingDamageEvent(float damage)
    {
        DoingDamageEvent?.Invoke(damage);
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

    public static event Action<int> GoldAmountChangeEvent;
    public static void OnGoldAmountChangeEvent(int goldAmount)
    {
        GoldAmountChangeEvent?.Invoke(goldAmount);
    }

    public static event Action<bool, RectTransform, SkillSO> ShowSkillToolTipEvent;
    public static void OnShowSkillToolTipEvent(bool show, RectTransform targetRect, SkillSO skillSO)
    {
        ShowSkillToolTipEvent?.Invoke(show, targetRect, skillSO);
    }

    public static event Action<bool, RectTransform, Inventory_Item, bool> ShowItemToolTipEvent;
    public static void OnShowItemToolTipEvent(bool show, RectTransform targetRect, Inventory_Item item, bool buyPrice)
    {
        ShowItemToolTipEvent?.Invoke(show, targetRect, item, buyPrice);
    }


    //消耗品使用事件
    public static event Action<float> IncreaseHealthEvent;
    public static void OnIncreaseHealthEvent(float healAmount)
    {
        IncreaseHealthEvent?.Invoke(healAmount);
    }

    public static event Action<Sprite, float> UpdateBuffIconEvent;
    public static void OnUpdateBuffIconEvent(Sprite buffIcon, float duration)
    {
        UpdateBuffIconEvent?.Invoke(buffIcon, duration);
    } 

    //技能触发事件
    public static event Action<float, float> SlowDownTargetEvent;
    public static void OnSlowDownTargetEvent(float duration, float slowMultiplier)
    {
        SlowDownTargetEvent?.Invoke(duration, slowMultiplier);
    }
}
