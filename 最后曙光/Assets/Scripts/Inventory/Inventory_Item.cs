using System;
using System.Text;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    private string itemId;
    public ItemDataSO itemData;
    public int stackSize = 1;

    public SkillSO skill { get; private set; }
    public ItemEffectDataSO itemEffectByConsumable { get; private set; }
    public ItemEffectDataSO itemEffectByEquipment { get; private set; }
    public ItemModifier[] modifiers { get; private set; }

    public int buyPrice { get; private set; }
    public float sellPrice { get; private set; }

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        skill = SkillScrollData()?.bindSkill;
        itemEffectByConsumable = ConsumableItemData()?.itemEffect;
        itemEffectByEquipment = EquipmentData()?.itemEffect;
        modifiers = EquipmentData()?.modifiers;

        buyPrice = itemData.itemPrice;
        sellPrice = itemData.itemPrice * .6f;

        itemId = itemData.itemName + " - " +  Guid.NewGuid();
    }

    public void AddModifiers(Player_Stats playerStats)
    {
        foreach(var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemId);
        }
    }

    public void RemoveModifiers(Player_Stats playerStats)
    {
        foreach(var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(itemId);
        }
    }

    public void AddItemEffect(Player player) => itemEffectByEquipment?.Subscribe(player);
    public void RemoveItemEffect() => itemEffectByEquipment?.Unsubscribe();

    public void LearnSkill(Player player, out bool result)
    {
        result = player.skill.LearnSkill(skill);
    }

    private SkillScrollDataSO SkillScrollData()
    {
        if(itemData is SkillScrollDataSO skillScroll)
            return skillScroll;
        
        return null;
    }

    private ConsumableItemDataSO ConsumableItemData()
    {
        if(itemData is ConsumableItemDataSO consumable)
            return consumable;
        
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

    public string GetItemInfo()
    {

        if(itemData.itemType == ItemType.Material)
            return "可用于合成";

        if(itemData.itemType == ItemType.SkillScroll)
        {
            return "记录着 " + skill.skillName + "LV." + skill.skillLevel + " 的远古卷轴";
        }

        StringBuilder sb = new StringBuilder();
        
        if(itemData.itemType == ItemType.Consumable)
        {
            sb.AppendLine("");
            sb.AppendLine(itemEffectByConsumable.effectDescription);
            return sb.ToString();
        }

        sb.AppendLine("");

        foreach(var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine("+" + modValue + " " + modType);
        }

        if(itemEffectByEquipment != null)
        {
            sb.AppendLine("");
            sb.AppendLine("装备特殊效果");
            sb.AppendLine(itemEffectByEquipment.effectDescription);
        }

        return sb.ToString();
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "最大生命值";
            case StatType.HealthRegen: return "生命回复";
            case StatType.Strength: return "力量";
            case StatType.Agility: return "敏捷";
            case StatType.Intelligence: return "智力";
            case StatType.Vitality: return "体质";
            case StatType.AttackSpeed: return "攻击速度";
            case StatType.Damage: return "攻击力";
            case StatType.CritChance: return "暴击率";
            case StatType.CritPower: return "暴击伤害";
            case StatType.ArmorReduction: return "破甲";
            case StatType.Armor: return "护甲";
            case StatType.Evasion: return "闪避率";
            default: return "未知属性";
        }
    }

    private bool IsPercentageStat(StatType type)
    {
        switch(type)
        {
            case StatType.Evasion:
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.AttackSpeed:
                return true;
            default:
                return false;
        }
    }
}
