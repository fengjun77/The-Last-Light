using System;
using UnityEngine;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat attackSpeed; //攻击速度

    public Stat damage; //基础伤害
    public Stat critPower; //暴击伤害
    public Stat critChance; //暴击率
    public Stat armorReduction; //护甲穿透

    public Stat fireDamage; //火焰伤害
    public Stat iceDamage; //冰霜伤害
    public Stat lightningDamage; //雷电伤害
}
