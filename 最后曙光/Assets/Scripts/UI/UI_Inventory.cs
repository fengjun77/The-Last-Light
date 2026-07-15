using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_ItemSlot[] uiItemSlots;
    private UI_EquipSlot[] uiEquipSlots;

    [SerializeField] private Transform uiItemSlotParent;
    [SerializeField] private Transform uiEquipSlotParent;

    void Awake()
    {
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();
        inventory = FindFirstObjectByType<Inventory_Player>();

        UpdateUI();
    }

    void OnEnable()
    {
        EventCenter.InventoryChangeEvent += UpdateUI;
    }

    void OnDisable()
    {
        EventCenter.InventoryChangeEvent -= UpdateUI;
    }

    private void UpdateUI()
    {
        UpdateInventorySlots();
        UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> playerEquipList = inventory.equipList;

        for(int i = 0; i < uiEquipSlots.Length; i++)
        {
            var playerEquipSlot = playerEquipList[i];

            if(!playerEquipSlot.HasItem())
            {
                uiEquipSlots[i].UpdateSlot(null);
            }
            else
            {
                uiEquipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
            }
        }
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
