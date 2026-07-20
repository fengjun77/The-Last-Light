using System;
using UnityEngine;

[Serializable]
public class Stat_MajorGroup
{
    public Stat strength; //强壮 每点增加1点伤害和0.6%暴击伤害
    public Stat agility; //敏捷 每点增加0.5%闪避率和0.3%暴击率
    public Stat intelligence; //智力 每点增加0.5%护甲穿透
    public Stat vitality; //活力 每点增加5点血量上限和2点护甲值
}
