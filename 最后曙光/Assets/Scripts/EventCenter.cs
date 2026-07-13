using System;
using UnityEngine;

public static class EventCenter
{
    public static event Action OnPlayerDeath;

    public static void OnPlayerDeathEvent()
    {
        OnPlayerDeath?.Invoke();
    }

    public static event Action<float, float> OnHealthChange;
    public static void OnHealthChangeEvent(float currentHp, float maxHP)
    {
        OnHealthChange?.Invoke(currentHp, maxHP);
    }
    public static event Action OnFlip;
    public static void OnFlipEvent()
    {
        OnFlip?.Invoke();
    }
}
