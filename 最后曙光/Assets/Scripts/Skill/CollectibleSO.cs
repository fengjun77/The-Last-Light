using UnityEngine;

public abstract class CollectibleSO : ScriptableObject
{
    [Header("通用")]
    public string collectibleName;
    public Sprite icon;
    public LootType type; 
    public abstract void Collect(Player player);
}

public enum LootType
{
    item,
    skill
}
