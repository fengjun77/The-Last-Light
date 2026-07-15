using System;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    private string itemId;
    public ItemDataSO itemData;
    public int stackSize = 1;

    public SkillSO skill { get; private set; }
    public ItemModifier[] modifiers { get; private set; }

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        skill = SkillScrollData()?.bindSkill;
        modifiers = EquipmentData()?.modifiers;

        itemId = itemData.itemName + " - " +  Guid.NewGuid();
    }

    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach(var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemId);
        }
    }

    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach(var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(itemId);
        }
    }

    public void LearnSkill(Player player, out bool result)
    {
        result = player.skillManager.LearnSkill(skill);
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
