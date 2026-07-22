using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    private Player_Stats playerStats;
    private RectTransform rect;

    [SerializeField] private StatType statSlotType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(statSlotType);
        statName.text = GetStatNameByType(statSlotType);
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        playerStats = FindAnyObjectByType<Player_Stats>();
    }

    /// <summary>
    /// 更新属性显示
    /// </summary>
    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statSlotType);

        if(statToUpdate == null) return;

        float value = 0;

        switch(statSlotType)
        {
            case StatType.Strength:
                value = playerStats.major.strength.GetValue();
                break;
            case StatType.Agility:
                value = playerStats.major.agility.GetValue();
                break;
            case StatType.Intelligence:
                value = playerStats.major.intelligence.GetValue();
                break;
            case StatType.Vitality:
                value = playerStats.major.vitality.GetValue();
                break;
            case StatType.AttackSpeed:
                value = playerStats.offense.attackSpeed.GetValue();
                break;
            case StatType.Damage:
                value = playerStats.GetBaseDamage();
                break;
            case StatType.CritChance:
                value = playerStats.GetCritChance();
                break;
            case StatType.CritPower:
                value = playerStats.GetCritPower();
                break;
            case StatType.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100;
                break;
            case StatType.Armor:
                value = playerStats.GetArmor();
                break;
            case StatType.Evasion:
                value = playerStats.GetEvasion();
                break;
            case StatType.MaxHealth:
                value = playerStats.GetMaxHealth();
                break;
            case StatType.HealthRegen:
                value = playerStats.resource.healthRegen.GetValue();
                break;
            default:
                return;
        }

        statValue.text = IsPercentageStat(statSlotType) ? value + "%" : value.ToString();
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
