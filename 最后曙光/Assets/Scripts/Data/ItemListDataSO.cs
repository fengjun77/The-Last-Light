using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Item Data/Item List", fileName = "List of items - ")]
public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;
}
