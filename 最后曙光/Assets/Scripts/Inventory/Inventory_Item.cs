using System;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    public ItemDataSO itemData;
    public int stackSize = 1;

    public SkillSO skill { get; private set; }
    public ItemModifier[] modifiers { get; private set; }

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        skill = SkillScrollData()?.bindSkill;
        modifiers = EquipmentData()?.modifiers;
    }

    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach(var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemData.itemName);
        }
    }

    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach(var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(itemData.itemName);
        }
    }

    public void LearnSkill(Player player)
    {
        player.skillManager.LearnSkill(skill);
    }

    private SkillScrollDataSO SkillScrollData()
    {
        if(itemData is SkillScrollDataSO skillScroll)
            return skillScroll;
        
        return null;
    }

    private EquipmentDataSO EquipmentData()
    {
        if(itemData is EquipmentDataSO equipment)
            return equipment;

        return null;
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
