using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Item Data/Item List", fileName = "List of items - ")]
public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;

    public ItemDataSO GetItemData(string saveID)
    {
        return itemList.FirstOrDefault(item => item != null && item.saveID == saveID);
    }

#if UNITY_EDITOR
    [ContextMenu("自动添加所有物品")]
    public void CollectItemsData()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");

        itemList = guids
                .Select((guid => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid))))
                .Where(item => item != null)
                .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
