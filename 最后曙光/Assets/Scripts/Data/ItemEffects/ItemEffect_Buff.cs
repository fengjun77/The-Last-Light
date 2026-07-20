using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/ItemEffect/Buff", fileName = "BuffEffect - ")]
public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private Sprite buffIcon;
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float duration;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    private Player_Stats playerStats;

    public override bool CanBeUsed()
    {
        if(playerStats == null)
            playerStats = FindFirstObjectByType<Player_Stats>();

        return playerStats.CanApplyBuffOf(source);
    }

    public override void ExecuteEffect()
    {
        playerStats.ApplyBuff(buffsToApply, duration, source);
        //更新BuffUI
        EventCenter.OnUpdateBuffIconEvent(buffIcon, duration);
    }
}
