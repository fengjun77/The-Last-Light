using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Item/Equipment", fileName = "Equipment - ")]
public class EquipmentDataSO : ItemDataSO
{
    [Header("物品属性")]
    public ItemModifier[] modifiers;
}

[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
}
