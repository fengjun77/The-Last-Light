using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/ItemEffect/Buff", fileName = "BuffEffect - ")]
public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private Sprite buffIcon;
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float duration;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    public override bool CanBeUsed(Player player)
    {
        if(player.stats.CanApplyBuffOf(source))
        {
            this.player = player;
            return true;
        }
        else
            return false;
    }

    public override void ExecuteEffect()
    {
        player.stats.ApplyBuff(buffsToApply, duration, source);
        //更新BuffUI
        EventCenter.OnUpdateBuffIconEvent(buffIcon, duration);
        player = null;
    }
}
