using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;

    [SerializeField] private UI_ItemSlotParent inventorySlotsParent;
    [SerializeField] private UI_EquipSlotParent equipSlotParent;

    void Awake()
    {
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
        inventorySlotsParent.UpdateSlots(inventory.itemList);
        equipSlotParent.UpdateEquipmentSlots(inventory.equipList);
        
    }
}
