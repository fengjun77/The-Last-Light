using System;
using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Merchant merchant;

    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_EquipSlotParent equipSlots;

    /// <summary>
    /// 打开商店时调用
    /// </summary>
    /// <param name="merchant"></param>
    /// <param name="inventory"></param>
    public void SetMerchantUI(Inventory_Merchant merchant, Inventory_Player inventory)
    {
        this.merchant = merchant;
        this.inventory = inventory;

        UpdateSlotUI();

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>(true);

        foreach(var slot in merchantSlots)
        {
            slot.SetupMerchantUI(merchant);
        }
    }

    /// <summary>
    /// 更新商店商品
    /// </summary>
    public void UpdateMerchantList()
    {
        inventory.gold -= 100;
        EventCenter.OnGoldAmountChangeEvent(inventory.gold);
        merchant.FillShopList();
    }

    void OnEnable()
    {
        EventCenter.InventoryChangeEvent += UpdateSlotUI;
    }

    void OnDisable()
    {
        EventCenter.InventoryChangeEvent -= UpdateSlotUI;
    }

    private void UpdateSlotUI()
    {
        if(inventory == null) return;

        inventorySlots.UpdateSlots(inventory.itemList);
        merchantSlots.UpdateSlots(merchant.itemList);
        equipSlots.UpdateEquipmentSlots(inventory.equipList);
    }

    
}
