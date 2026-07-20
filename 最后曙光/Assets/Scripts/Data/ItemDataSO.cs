using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Item/Material", fileName = "Material - ")]
public class ItemDataSO : ScriptableObject
{
    [Header("交易详情")]
    [Range(0,10000)]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 1;

    [Header("掉落详情")]
    [Range(0, 1000)]
    public int itemRarity = 100; //物品稀有度（越高越难出）
    [Range(0, 100)]
    public float dropChance; //掉落概率
    [Range(0, 100)]
    public float maxDropChance = 65f; //最大概率

    [Header("物品详情")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header("制作详情")]
    public Inventory_Item[] crafeRecipe;

    public void OnValidate()
    {
        dropChance = GetDropChance();
    }

    public float GetDropChance()
    {
        float maxRarity = 1000;
        float chance = (maxRarity - itemRarity + 1) / maxRarity * 100;

        return Mathf.Min(chance, maxDropChance);
    }
}

public enum ItemType
{
    Material, //材料
    Weapon, //武器
    Armor, //防具
    Trinket, //饰品
    SkillScroll, //技能卷轴
    Consumable, //消耗品
}
