using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Item/Consumable", fileName = "Consumable - ")]
public class ConsumableItemDataSO : ItemDataSO
{
    [Header("使用效果")]
    public ItemEffectDataSO itemEffect;
}
