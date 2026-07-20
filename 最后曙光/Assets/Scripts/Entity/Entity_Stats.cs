using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;

    public Stat_ResourceGroup resource;

    public Stat_MajorGroup major;

    public Stat_OffenseGroup offense;

    public Stat_DefenseGroup defense;

    public float GetPhysicalDamage(out bool isCrit)
    {
        float baseDamage = GetBaseDamage();

        float critChance = GetCritChance();

        float critPower = GetCritPower() / 100;

        isCrit = Random.Range(0, 100) < critChance;

        float finalDamage = isCrit ? baseDamage * (1 + critPower) : baseDamage;

        return finalDamage;
    }

    public float GetBaseDamage() => offense.damage.GetValue() + major.strength.GetValue();
    public float GetCritChance() => offense.critChance.GetValue() + major.agility.GetValue() * .3f;
    public float GetCritPower() => offense.critPower.GetValue() + major.strength.GetValue() * .6f;
    
    public float GetArmorReduction()
    {
        float baseArmorReduction = offense.armorReduction.GetValue();
        float bonusArmorReduction = major.intelligence.GetValue() * .5f;

        float reductionCap = 60f;
        float totalArmorReduction = baseArmorReduction + bonusArmorReduction;
        float finalReduction = Mathf.Clamp(totalArmorReduction, 0, reductionCap) / 100;

        return finalReduction;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = GetArmor();
        
        float reductionMutliplier = Mathf.Clamp01(1 - armorReduction);
        float effectiveArmor = baseArmor * reductionMutliplier;

        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = .7f;

        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);

        return finalMitigation;
    }

    public float GetArmor() => defense.armor.GetValue() + major.vitality.GetValue() * 2;

    public float GetMaxHealth()
    {
        float baseMaxHealth = resource.maxHealth.GetValue();
        float bonusMaxHealth = major.vitality.GetValue() * 5;

        float finalMaxHealth = baseMaxHealth + bonusMaxHealth;

        return finalMaxHealth;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = major.agility.GetValue() * .5f;   

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 40f; //闪避上限

        float finalEvasion = Mathf.Clamp(totalEvasion, 0, evasionCap);

        return finalEvasion;
    }

    public Stat GetStatByType(StatType type)
    {
        switch(type)
        {
            case StatType.MaxHealth:
                return resource.maxHealth;
            case StatType.HealthRegen:
                return resource.healthRegen;
            case StatType.Strength:
                return major.strength;
            case StatType.Agility:
                return major.agility;
            case StatType.Intelligence:
                return major.intelligence;
            case StatType.Vitality:
                return major.vitality;
            case StatType.AttackSpeed:
                return offense.attackSpeed;
            case StatType.Damage:
                return offense.damage;
            case StatType.CritChance:
                return offense.critChance;
            case StatType.CritPower:
                return offense.critPower;
            case StatType.ArmorReduction:
                return offense.armorReduction;
            case StatType.Armor:
                return defense.armor;
            case StatType.Evasion:
                return defense.evasion;
            default:
                Debug.Log("没有找到该属性");
                return null;
        }
    }

    [ContextMenu("更新角色属性")]
    public void ApplyDefaultStatSetup()
    {
        if(defaultStatSetup == null)
        {
            Debug.Log("没有默认属性配置");
            return;
        }

        resource.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resource.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);

        major.strength.SetBaseValue(defaultStatSetup.strength);
        major.agility.SetBaseValue(defaultStatSetup.agility);
        major.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        major.vitality.SetBaseValue(defaultStatSetup.vitality);

        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);
        offense.critChance.SetBaseValue(defaultStatSetup.critChance);
        offense.critPower.SetBaseValue(defaultStatSetup.critPower);

        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);  
    }
}
