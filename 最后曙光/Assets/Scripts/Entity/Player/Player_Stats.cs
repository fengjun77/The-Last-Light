using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    public List<string> activeBuff = new List<string>();

    /// <summary>
    /// 是否可以施加该buff（只有在没有该buff的时候才可施加）
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool CanApplyBuffOf(string source)
    {
        return activeBuff.Contains(source) == false;
    }

    public void ApplyBuff(BuffEffectData[] buffsToApply, float duration, string source)
    {
        StartCoroutine(BuffCo(buffsToApply, duration, source));
    }

    private IEnumerator BuffCo(BuffEffectData[] buffsToApply, float duration, string source)
    {
        activeBuff.Add(source);

        foreach(var buff in buffsToApply)
        {
            GetStatByType(buff.buffType).AddModifier(buff.buffValue, source);
        }

        yield return new WaitForSeconds(duration);

        foreach(var buff in buffsToApply)
        {
            GetStatByType(buff.buffType).RemoveModifier(source);
        }

        //更新UI
        EventCenter.OnInventoryChangeEvent();

        activeBuff.Remove(source);
    }
}
