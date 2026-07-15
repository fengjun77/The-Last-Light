using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Item/Material", fileName = "Material - ")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;
}

public enum ItemType
{
    Material, //材料
    Weapon, //武器
    Armor, //防具
    Trinket, //饰品
    SkillScroll, //技能卷轴
}
