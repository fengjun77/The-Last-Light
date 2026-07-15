using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private UI_ItemSlot[] uiItemSlots;
    private Inventory_Base inventory;

    void Awake()
    {
        uiItemSlots = GetComponentsInChildren<UI_ItemSlot>();
        inventory = FindFirstObjectByType<Inventory_Base>();

        UpdateInventorySlots();
    }

    void OnEnable()
    {
        EventCenter.InventoryChangeEvent += UpdateInventorySlots;
    }

    void OnDisable()
    {
        EventCenter.InventoryChangeEvent -= UpdateInventorySlots;
    }

    /// <summary>
    /// 更新所有格子信息
    /// </summary>
    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = inventory.itemList;

        for(int i = 0; i < uiItemSlots.Length; i++)
        {
            if(i < itemList.Count)
            {
                uiItemSlots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                uiItemSlots[i].UpdateSlot(null);
            }
        } 
    }
}
